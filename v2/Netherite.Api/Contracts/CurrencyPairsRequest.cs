#nullable enable
namespace Netherite.API.Contracts
{
	public class CurrencyPairsRequest
    {
        public CurrencyPairsRequest() 
        {
        }

		public CurrencyPairsRequest(string Name, string NameTwo)
        {            
            this.Symbol = Name;
            
            this.SymbolTwo = NameTwo;                       
        }       

        public string Symbol { get; init; }

        public string SymbolTwo { get; init; }        	
    }
}
