
namespace Netherite.Infrastructure.Entities
{
	public class CurrencyPairsIntervalEntity
	{
		public Guid Id { get; set; }

		public Guid CurrencyPairsId { get; set; }

		public CurrencyPairsEntity CurrencyPairs { get; set; } = null;

		public Guid IntervalId { get; set; }

		public IntervalEntity Interval { get; set; } = null;
		
	}
}
