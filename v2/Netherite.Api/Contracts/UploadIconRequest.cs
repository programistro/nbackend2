using Netherite.Domain.Models;

namespace Netherite.Api.Contracts
{
	public class UploadIconRequest
	{
		public UploadIconRequest(Guid currencyPairId, string symbol)
		{
			this.CurrencyPairId = currencyPairId;
			this.Symbol = symbol;
		}

		public Guid CurrencyPairId { get; init; }

		public string Symbol { get; init; }
	}
}
