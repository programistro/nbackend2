namespace Netherite.Infrastructure.Entities
{
	public class CurrencyPairsEntity
	{
		public Guid Id { get; set; }

		public string Symbol { get; set; }

		public string SymbolTwo { get; set; }

		public string Icon { get; set; }	

		public ICollection<CurrencyPairsIntervalEntity> CurrencyPairsIntervals { get; set; } = new List<CurrencyPairsIntervalEntity>();

		public ICollection<FavoritesEntity> Favorites { get; set; } = new List<FavoritesEntity>();		
	}
}
