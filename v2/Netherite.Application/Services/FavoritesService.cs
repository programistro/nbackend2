
using Netherite.Domain.Models;
using Netherite.Domain.Repositories;
using Netherite.Domain.Services;
using System;
using System.Collections.Generic;


#nullable enable
namespace Netherite.Application.Services
{
    public class FavoritesService : IFavoritesService
    {
        private readonly IFavoritesRepository _favoritesRepository;

        public FavoritesService(IFavoritesRepository favoritesRepository) => this._favoritesRepository = favoritesRepository;

        public System.Threading.Tasks.Task<Guid> CreateFavorites(
          Guid userId,
          Guid currencyPairsId)
        {
            return this._favoritesRepository.Create(userId, currencyPairsId);
        }

        public System.Threading.Tasks.Task<bool> DeleteFavorites(Guid userId, Guid currencyPairsId) => this._favoritesRepository.Delete(userId, currencyPairsId);

        public System.Threading.Tasks.Task<List<CurrencyPairs>> GetFavorites(
          Guid userId)
        {
            return this._favoritesRepository.Get(userId);
        }
    }
}
