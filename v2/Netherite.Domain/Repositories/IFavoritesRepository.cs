using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Netherite.Domain.Models;

namespace Netherite.Domain.Repositories
{
	public interface IFavoritesRepository
	{
		Task<List<CurrencyPairs>> Get(Guid userId);

		Task<Guid> Create(Guid userId, Guid currencyPairsId);

		Task<bool> Delete(Guid userId, Guid currencyPairsId);
	}
}
