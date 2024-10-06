using Netherite.Domain.Models;
using Task = System.Threading.Tasks.Task;

namespace Netherite.Domain.Services
{
	public interface IOrderServices
	{
		Task<DateTime> CreateOrder(Order order);

		Task<List<Order>> GetOrders(Guid userId);

		Task CompleteOrderAfterDelay(Guid orderId, decimal interestRate);
	}
}
