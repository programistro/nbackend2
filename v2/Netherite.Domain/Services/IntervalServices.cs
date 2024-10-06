using Netherite.Domain.Models;

namespace Netherite.Domain.Services
{
	public interface IIntervalServices
	{
		Task<List<Interval>> GetIntervalByPairsId(Guid pairsId);

		Task<Guid> CreateInterval(Interval interval, Guid pairsId);

		Task<bool> DeleteInterval(Guid intervalId);

		Task<bool> UpdateInterval(Guid intervalId, Interval interval);

		Task<Interval> GetIntervalById(Guid intervalId);
	}
}
