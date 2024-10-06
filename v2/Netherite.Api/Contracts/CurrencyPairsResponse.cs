namespace Netherite.API.Contracts
{
	public class CurrencyPairsResponse 
	{
		public CurrencyPairsResponse(Guid Id, string Name, string NameTwo, string Icon)
		{
			this.Id = Id;
			this.Symbol = Name;
			this.SymbolTwo = NameTwo;
			this.Icon = Icon;
		}		

		public Guid Id { get; set; }

		public string Symbol { get; set; }

		public string SymbolTwo { get; set; }

		public string Icon { get; set; }		
		
	}
}
