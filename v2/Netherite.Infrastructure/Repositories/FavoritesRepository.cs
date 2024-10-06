
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Netherite.Domain.Models;
using Netherite.Domain.Repositories;
using Netherite.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;


#nullable enable
namespace Netherite.Infrastructure.Repositories
{
    public class FavoritesRepository : IFavoritesRepository
    {
        private readonly NetheriteDbContext _context;

        public FavoritesRepository(NetheriteDbContext context) => this._context = context;

        public async System.Threading.Tasks.Task<List<CurrencyPairs>> Get(
          Guid userId)
        {
            List<FavoritesEntity> favoritesEntity = await (System.Threading.Tasks.Task<List<FavoritesEntity>>)EntityFrameworkQueryableExtensions.ToListAsync<FavoritesEntity>(EntityFrameworkQueryableExtensions.Include<FavoritesEntity, CurrencyPairsEntity>(((IQueryable<FavoritesEntity>)this._context.Favorites).Where<FavoritesEntity>((Expression<Func<FavoritesEntity, bool>>)(f => f.UserId == userId)), (f => f.CurrencyPairs)), new CancellationToken());
            List<CurrencyPairs> currencyPairs = favoritesEntity.Select<FavoritesEntity, CurrencyPairs>((Func<FavoritesEntity, CurrencyPairs>)(f => CurrencyPairs.Create(f.CurrencyPairs.Id, f.CurrencyPairs.Symbol, f.CurrencyPairs.SymbolTwo, f.CurrencyPairs.Icon))).ToList<CurrencyPairs>();
            List<CurrencyPairs> currencyPairsList = currencyPairs;
            favoritesEntity = (List<FavoritesEntity>)null;
            currencyPairs = (List<CurrencyPairs>)null;
            return currencyPairsList;
        }

        public async System.Threading.Tasks.Task<Guid> Create(Guid userId, Guid currencyPairsId)
        {
            CurrencyPairsEntity currencyPairsEntity = await (System.Threading.Tasks.Task<CurrencyPairsEntity>)EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<CurrencyPairsEntity>(this._context.CurrencyPairs, (cp => cp.Id == currencyPairsId), new CancellationToken());
            UserEntity userEntity = await (System.Threading.Tasks.Task<UserEntity>)EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<UserEntity>(this._context.Users, (u => u.Id == userId), new CancellationToken());
            if (currencyPairsEntity == null || userEntity == null)
                return Guid.Empty;
            FavoritesEntity favoritesEntity = new FavoritesEntity()
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                CurrencyPairsId = currencyPairsId,
                User = userEntity,
                CurrencyPairs = currencyPairsEntity
            };
            EntityEntry<FavoritesEntity> entityEntry = await this._context.Favorites.AddAsync(favoritesEntity, new CancellationToken());
            int num = await this._context.SaveChangesAsync(new CancellationToken());
            return favoritesEntity.Id;
        }

        public async System.Threading.Tasks.Task<bool> Delete(Guid userId, Guid currencyPairsId)
        {
            FavoritesEntity favoritesEntity = await (System.Threading.Tasks.Task<FavoritesEntity>)EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<FavoritesEntity>(this._context.Favorites, (f => f.UserId == userId && f.CurrencyPairsId == currencyPairsId), new CancellationToken());
            if (favoritesEntity == null)
                return false;
            this._context.Favorites.Remove(favoritesEntity);
            int num = await this._context.SaveChangesAsync(new CancellationToken());
            return true;
        }
    }
}
