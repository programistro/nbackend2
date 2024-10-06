using Netherite.Domain.Models;
using Netherite.Domain.Repositories;
using Netherite.Domain.Services;
using Task = System.Threading.Tasks.Task;

namespace Netherite.Application.Services
{
	public class OrderService : IOrderServices
	{
		public OrderService(IOrderRepository orderRepository)
		{
			this._orderRepository = orderRepository;
		}

		public async Task<DateTime> CreateOrder(Order order)
		{
			return await this._orderRepository.CreateOrder(order);
		}

		public async Task<List<Order>> GetOrders(Guid userId)
		{
			return await this._orderRepository.GetOrders(userId);
		}

		public async Task CompleteOrderAfterDelay(Guid orderId, decimal interestRate)
		{
			await this._orderRepository.CompleteOrderAfterDelay(orderId, interestRate);
		}

		private readonly IOrderRepository _orderRepository;
	}
}
