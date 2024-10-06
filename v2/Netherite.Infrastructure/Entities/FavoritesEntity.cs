
namespace Netherite.Infrastructure.Entities
{
	public class FavoritesEntity
	{
		public Guid Id { get; set; }

		public Guid UserId { get; set; }

		public Guid CurrencyPairsId { get; set; }

		public UserEntity User { get; set; } = null;

		public CurrencyPairsEntity CurrencyPairs { get; set; } = null;
	}
}
