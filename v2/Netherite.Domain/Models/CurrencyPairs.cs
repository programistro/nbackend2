
namespace Netherite.Domain.Models
{
	public class CurrencyPairs
	{
		private CurrencyPairs(Guid id, string name, string nameTwo, string icon)
		{
			this.Id = id;
			this.Symbol = name;
			this.SymbolTwo = nameTwo;
			this.Icon = icon;			
		}

		public Guid Id { get; set; }

		public string Symbol { get; set; }

		public string SymbolTwo { get; set; }

		public string Icon { get; set; }		

		public static CurrencyPairs Create(Guid id, string symbol, string symbolTwo, string icon)
		{
			return new CurrencyPairs(id, symbol, symbolTwo, icon);
		}
	}
}
