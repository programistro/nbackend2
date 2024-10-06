using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Netherite.Domain.Models;

namespace Netherite.Domain.Services
{
	public interface ICurrencyPairsService
	{
		Task<List<CurrencyPairs>> GetCurrencyPairs();

		Task<Guid> CreateCurrencyPairs(CurrencyPairs currencyPairs);

		Task<bool> DeleteCurrencyPairs(Guid currencyPairsId);

		Task<bool> UpdateCurrencyPairs(Guid currencyPairsId, CurrencyPairs currencyPairs);

		Task<bool> UploadIcon(Guid currencyPairId, string fileUrl);
	}
}
