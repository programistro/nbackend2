using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Netherite.Domain.Models;

namespace Netherite.Domain.Services
{
	public interface IFavoritesService
	{
		Task<List<CurrencyPairs>> GetFavorites(Guid userId);

		Task<Guid> CreateFavorites(Guid userId, Guid currencyPairsId);

		Task<bool> DeleteFavorites(Guid userId, Guid currencyPairsId);
	}
}
