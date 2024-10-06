using System;
using System.Runtime.CompilerServices;

namespace Netherite.Domain.Models
{
	public class Order
	{
		private Order(Guid id, Guid userId, Guid currencyPairsId, Guid intervalId, int bet, decimal startPrice, DateTime startTime, DateTime endTime, bool purchaseDirection, bool ended)
		{
			this.Id = id;
			this.UserId = userId;
			this.CurrencyPairsId = currencyPairsId;
			this.IntervalId = intervalId;
			this.Bet = bet;
			this.StartPrice = startPrice;
			this.StartTime = startTime;
			this.EndTime = endTime;
			this.PurchaseDirection = purchaseDirection;
			this.Ended = ended;
		}

		public Guid Id { get; set; }

		public Guid UserId { get; set; }

		public Guid CurrencyPairsId { get; set; }

		public Guid IntervalId { get; set; }

		public int Bet { get; set; }

		public decimal StartPrice { get; set; }

		public DateTime StartTime { get; set; }

		public DateTime EndTime { get; set; }

		public bool PurchaseDirection { get; set; }

		public bool Ended { get; set; }

		public static Order Create(Guid id, Guid userId, Guid currencyPairsId, Guid intervalId, int bet, decimal startPrice, DateTime startTime, DateTime endTime, bool purchaseDirection, bool ended)
		{
			return new Order(id, userId, currencyPairsId, intervalId, bet, startPrice, startTime, endTime, purchaseDirection, ended);
		}
	}
}
