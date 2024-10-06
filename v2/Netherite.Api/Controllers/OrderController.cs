
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Netherite.API.Contracts;
using Netherite.Domain.Models;
using Netherite.Domain.Services;

#nullable enable
namespace Netherite.API.Controllers
{
	[ApiController]
    [Authorize]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderServices _orderServices;
		private readonly IIntervalServices _intervalServices;
		private readonly OrderBackgroundService _orderBackgroundService;

        public OrderController(
          IOrderServices orderServices,
		  IIntervalServices intervalServices,
		  OrderBackgroundService orderBackgroundService)
        {
            this._orderServices = orderServices;
			this._intervalServices = intervalServices;
			this._orderBackgroundService = orderBackgroundService;
        }
		
		[HttpPost("add")]
		public async System.Threading.Tasks.Task<ActionResult<Guid>> CreateOrder([FromBody] OrderRequest request)
        {
            try
            {
                Order order = Order.Create(Guid.NewGuid(), request.UserId, request.CurrencyPairsId, request.IntervalId, request.Bet, 0M, DateTime.UtcNow, DateTime.UtcNow, request.PurchaseDirection, false);
                DateTime orderDate = await this._orderServices.CreateOrder(order);

                var interval = await _intervalServices.GetIntervalById(order.IntervalId);
               
                await this._orderBackgroundService.ProcessOrderAsync(order.Id, interval.Time, interval.InterestRate);

                return this.Ok((object)order.Id);
            }
            catch (Exception ex)
            {
                return this.BadRequest((object)ex.Message);
            }
        }

        /// <summary>
        /// Получение списка ордеров
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        [AllowAnonymous]
		[HttpGet("{userId}")]
        public async System.Threading.Tasks.Task<ActionResult<List<OrderResponse>>> GetOrdersByUserId(
          Guid userId)
        {
            try
            {
                List<Order> orders = await this._orderServices.GetOrders(userId);
                if (orders == null)
                    return this.NotFound();
                IEnumerable<OrderResponse> response = orders.Select<Order, OrderResponse>((Func<Order, OrderResponse>)(t => new OrderResponse(t.Id, t.UserId, t.CurrencyPairsId, t.IntervalId, t.Bet, t.StartPrice, t.StartTime, t.EndTime, t.PurchaseDirection, t.Ended)));
                return this.Ok((object)response);
            }
            catch (Exception ex)
            {
                return this.BadRequest((object)ex.Message);
            }
        }
    }
}
