using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Netherite.Domain.Models;

namespace Netherite.Domain.Repositories
{
	public interface ICurrencyPairsRepository
	{
		Task<List<CurrencyPairs>> GetCurrencyPairs();

		Task<Guid> Create(CurrencyPairs currencyPairs);

		Task<bool> Delete(Guid currencyPairsId);

		Task<bool> Update(Guid currencyPairsId, CurrencyPairs currencyPairs);

		Task<bool> UploadIcon(Guid currencyPairId, string fileUrl);
	}
}
