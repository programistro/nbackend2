
namespace Netherite.Infrastructure.Entities
{
	public class IntervalEntity
	{
		public Guid Id { get; set; }

		public int Time { get; set; }

		public decimal InterestRate { get; set; }

		public ICollection<CurrencyPairsIntervalEntity> CurrencyPairsIntervals { get; set; } = new List<CurrencyPairsIntervalEntity>();
	}
}
