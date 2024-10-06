// Type: OrderRepository

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Netherite.Domain.Models;
using Netherite.Domain.Repositories;
using Netherite.Domain.Services;
using Netherite.Infrastructure;
using Netherite.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text.Json;


#nullable enable
public class OrderRepository : IOrderRepository
{
    private readonly NetheriteDbContext _context;
    private readonly HttpClient _httpClient;
    private readonly IReferalBonusesServices _referalBonusesServices;
	private readonly IIntervalServices _intervalServices;

	public OrderRepository(
      NetheriteDbContext context,
      HttpClient httpClient,
      IReferalBonusesServices referalBonusesServices,
      IIntervalServices intervalServices)
    {
        this._context = context;
        this._httpClient = httpClient;
        this._referalBonusesServices = referalBonusesServices;
        this._intervalServices = intervalServices;
    }

    public async System.Threading.Tasks.Task<DateTime> CreateOrder(Order order)
    {
        UserEntity userEntity = await this._context.Users.FindAsync(order.UserId);

        if (userEntity is null)
        {
           throw new Exception("User not found");
        }

        CurrencyPairsEntity currencyPairsEntity = await this._context.CurrencyPairs.FindAsync(order.CurrencyPairsId);
        if (currencyPairsEntity is null)
        {
            throw new Exception("Currency pair not found");
        }

        if (userEntity.Balance < (Decimal)order.Bet)
            throw new Exception("Недостаточно баланса");

        userEntity.Balance -= (Decimal)order.Bet;

        string currencyPairSymbol = currencyPairsEntity.Symbol.Replace("/", "");
		string currencyPairSymbolTwo = currencyPairsEntity.SymbolTwo.Replace("/", "");

		HttpResponseMessage response = await _httpClient.GetAsync("https://api.binance.com/api/v3/ticker/price?symbol=" + currencyPairSymbol + currencyPairSymbolTwo);

        if (!response.IsSuccessStatusCode)
            throw new Exception("Не удалось получить начальную цену с Binance");

        Stream utf8Json = await response.Content.ReadAsStreamAsync();
        BinancePriceResponse priceData = await JsonSerializer.DeserializeAsync<BinancePriceResponse>(utf8Json);              

        Console.WriteLine(priceData);

        order.StartPrice = Decimal.Parse(priceData.price, (IFormatProvider)CultureInfo.InvariantCulture);

        var interval = await _intervalServices.GetIntervalById(order.IntervalId);

        DateTime startTime = order.StartTime;
        DateTime endTime = startTime.AddSeconds((double)interval.Time);

        OrderEntity orderEntity = new OrderEntity()
        {
            Id = order.Id,
            UserId = order.UserId,
            CurrencyPairsId = order.CurrencyPairsId,
            IntervalId = order.IntervalId,
            Bet = order.Bet,
            StartPrice = order.StartPrice,
            StartTime = startTime,
            EndTime = endTime,
            PurchaseDirection = order.PurchaseDirection,
            Ended = false
        };

        EntityEntry<OrderEntity> entityEntry = await this._context.Orders.AddAsync(orderEntity);

        this._context.Users.Update(userEntity);
        int num = await this._context.SaveChangesAsync();
        
        return endTime;
    }

    public async System.Threading.Tasks.Task CompleteOrderAfterDelay(Guid orderId, Decimal interestRate)
    {
        OrderEntity orderEntity = await this._context.Orders.FindAsync((object)orderId);
        if (orderEntity != null)
        {
            Console.WriteLine("Сделка получена");
            UserEntity user = await this._context.Users.FindAsync((object)orderEntity.UserId);
            Console.WriteLine("Пользователь получен");
            CurrencyPairsEntity currencyPairEntity = await this._context.CurrencyPairs.FindAsync((object)orderEntity.CurrencyPairsId);
            if (currencyPairEntity == null)
            {
                orderEntity.Ended = true;
                user.Balance += (Decimal)orderEntity.Bet;
                int num = await this._context.SaveChangesAsync();
                Console.WriteLine("Валютная пара не найдена");
                orderEntity = (OrderEntity)null;
            }
            else
            {
                Console.WriteLine("Валютная пара найдена");

				string currencyPairSymbol = currencyPairEntity.Symbol.Replace("/", "");
				string currencyPairSymbolTwo = currencyPairEntity.SymbolTwo.Replace("/", "");

				HttpResponseMessage response = await this._httpClient.GetAsync("https://api.binance.com/api/v3/ticker/price?symbol=" + currencyPairSymbol + currencyPairSymbolTwo);

				if (!response.IsSuccessStatusCode)
                {
                    orderEntity.Ended = true;
                    user.Balance += (Decimal)orderEntity.Bet;
                    int num = await this._context.SaveChangesAsync();
                    orderEntity = (OrderEntity)null;
                }
                else
                {
                    Stream utf8Json = await response.Content.ReadAsStreamAsync();
                    BinancePriceResponse binancePriceResponse = await JsonSerializer.DeserializeAsync<BinancePriceResponse>(utf8Json);
                    utf8Json = (Stream)null;
                    BinancePriceResponse priceData = binancePriceResponse;
                    binancePriceResponse = (BinancePriceResponse)null;
                    Decimal currentPrice = Decimal.Parse(priceData.price, (IFormatProvider)CultureInfo.InvariantCulture);
                    if (orderEntity.PurchaseDirection && currentPrice > orderEntity.StartPrice)
                        await this.HandleWinningOrder(orderEntity, user, interestRate, true);
                    else if (!orderEntity.PurchaseDirection && currentPrice < orderEntity.StartPrice)
                        await this.HandleWinningOrder(orderEntity, user, interestRate, false);
                    else if (currentPrice == orderEntity.StartPrice)
                    {
                        user.Balance += (Decimal)orderEntity.Bet;
                        orderEntity.Ended = true;
                        int num = await this._context.SaveChangesAsync();
                        Console.WriteLine("Ставка ничья сыграла");
                    }
                    else
                        await this.HandleLosingOrder(orderEntity, user);
                }
            }
        }
        else
        {
            Console.WriteLine("Сделка не найдена");
            orderEntity = (OrderEntity)null;
        }
    }

    private async System.Threading.Tasks.Task HandleWinningOrder(
      OrderEntity orderEntity,
      UserEntity user,
      Decimal interestRate,
      bool isUpward)
    {
        Decimal profitFactor = interestRate / 100M;
        Decimal profit = (Decimal)orderEntity.Bet * profitFactor;
        Decimal winnings = (Decimal)orderEntity.Bet + profit;
        user.Balance += winnings;
        await this.ProcessReferralBonuses(user, profit);
        orderEntity.Ended = true;
        int num = await this._context.SaveChangesAsync();
        Console.WriteLine(isUpward ? "Ставка вверх сыграла" : "Ставка вниз сыграла");
    }

    private async System.Threading.Tasks.Task HandleLosingOrder(
      OrderEntity orderEntity,
      UserEntity user)
    {
        await this.ProcessReferralBonuses(user, (Decimal)orderEntity.Bet);
        orderEntity.Ended = true;
        int num = await this._context.SaveChangesAsync();
        Console.WriteLine("Ставка не сыграла");
    }

    private async System.Threading.Tasks.Task ProcessReferralBonuses(
      UserEntity user,
      Decimal amount)
    {
        UserEntity referrer = await this._context.Users.FirstOrDefaultAsync<UserEntity>((Expression<Func<UserEntity, bool>>)(u => (Guid?)u.Id == user.InvitedId));
        UserEntity referrersReferrer = (UserEntity)null;
        if (referrer != null)
            referrersReferrer = await this._context.Users.FirstOrDefaultAsync<UserEntity>((Expression<Func<UserEntity, bool>>)(u => (Guid?)u.Id == referrer.InvitedId));
        (int, int) valueTuple1 = await this._referalBonusesServices.Execute(user.IsPremium, (int)amount);
        (int, int) valueTuple2 = valueTuple1;
        int referrerReward = valueTuple2.Item1;
        int referrersReferrerReward = valueTuple2.Item2;
        //valueTuple1 = ();
        //valueTuple2 = ();
        if (referrer == null)
        {
            referrersReferrer = (UserEntity)null;
        }
        else
        {
            referrer.Balance += (Decimal)referrerReward;
            user.Profit += referrerReward;
            if (referrersReferrer != null)
            {
                referrersReferrer.Balance += (Decimal)referrersReferrerReward;
                referrer.Profit += referrersReferrerReward;
            }
            referrersReferrer = (UserEntity)null;
        }
    }

    public async System.Threading.Tasks.Task<List<Order>> GetOrders(Guid userId)
    {
        List<OrderEntity> orderEntities = await this._context.Orders.Where<OrderEntity>((Expression<Func<OrderEntity, bool>>)(o => o.UserId == userId)).ToListAsync<OrderEntity>();
        List<Order> orders = orderEntities.Select<OrderEntity, Order>((Func<OrderEntity, Order>)(o => Order.Create(o.Id, o.UserId, o.CurrencyPairsId, o.IntervalId, o.Bet, o.StartPrice, o.StartTime, o.EndTime, o.PurchaseDirection, o.Ended))).ToList<Order>();

        return orders;
    }
}
