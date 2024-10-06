using Netherite.Domain.Models;
using Task = System.Threading.Tasks.Task;

namespace Netherite.Domain.Repositories
{
	public interface IOrderRepository
	{
		Task<DateTime> CreateOrder(Order order);

		Task<List<Order>> GetOrders(Guid userId);

		Task CompleteOrderAfterDelay(Guid orderId, decimal interestRate);
	}
}

