
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Netherite.Domain.Models;
using Netherite.Domain.Repositories;
using Netherite.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;


#nullable enable
namespace Netherite.Infrastructure.Repositories
{
    public class CurrencyPairsRepository : ICurrencyPairsRepository
    {
        private readonly NetheriteDbContext _context;

        public CurrencyPairsRepository(NetheriteDbContext context) => this._context = context;

        public async System.Threading.Tasks.Task<List<CurrencyPairs>> GetCurrencyPairs()
        {
            List<CurrencyPairsEntity> currencyPairsEntities = await (System.Threading.Tasks.Task<List<CurrencyPairsEntity>>)EntityFrameworkQueryableExtensions.ToListAsync<CurrencyPairsEntity>(EntityFrameworkQueryableExtensions.AsNoTracking<CurrencyPairsEntity>(this._context.CurrencyPairs), new CancellationToken());
            List<CurrencyPairs> currencyPairs = currencyPairsEntities.Select<CurrencyPairsEntity, CurrencyPairs>((Func<CurrencyPairsEntity, CurrencyPairs>)(cp => CurrencyPairs.Create(cp.Id, cp.Symbol, cp.SymbolTwo, cp.Icon))).ToList<CurrencyPairs>();
            List<CurrencyPairs> currencyPairsList = currencyPairs;
            currencyPairsEntities = (List<CurrencyPairsEntity>)null;
            currencyPairs = (List<CurrencyPairs>)null;
            return currencyPairsList;
        }

        public async System.Threading.Tasks.Task<Guid> Create(CurrencyPairs currencyPairs)
        {
            var currencyPairsEntity = new CurrencyPairsEntity()
            {
                Id = currencyPairs.Id,
                Symbol = currencyPairs.Symbol,
                SymbolTwo = currencyPairs.SymbolTwo,
                Icon = currencyPairs.Icon
            };
            var entityEntry = await this._context.CurrencyPairs.AddAsync(currencyPairsEntity, new CancellationToken());
            int num = await this._context.SaveChangesAsync(new CancellationToken());
            Guid id = currencyPairsEntity.Id;
            currencyPairsEntity = (CurrencyPairsEntity)null;
            return id;
        }

        public async System.Threading.Tasks.Task<bool> Delete(Guid currencyPairsId)
        {
            CurrencyPairsEntity currencyPairsEntity = await this._context.CurrencyPairs.FindAsync(new object[1]
            {
        (object) currencyPairsId
            });
            if (currencyPairsEntity == null)
                return false;
            this._context.CurrencyPairs.Remove(currencyPairsEntity);
            int num = await this._context.SaveChangesAsync(new CancellationToken());
            return true;
        }

        public async System.Threading.Tasks.Task<bool> Update(
          Guid currencyPairsId,
          CurrencyPairs currencyPairs)
        {
            CurrencyPairsEntity currencyPairsEntity = await this._context.CurrencyPairs.FindAsync(new object[1]
            {
        (object) currencyPairsId
            });
            if (currencyPairsEntity == null)
                return false;
            if (!string.IsNullOrEmpty(currencyPairs.Symbol))
                currencyPairsEntity.Symbol = currencyPairs.Symbol;
            if (!string.IsNullOrEmpty(currencyPairs.SymbolTwo))
                currencyPairsEntity.SymbolTwo = currencyPairs.SymbolTwo;
            if (!string.IsNullOrEmpty(currencyPairs.Icon))
                currencyPairsEntity.Icon = currencyPairs.Icon;           
            this._context.CurrencyPairs.Update(currencyPairsEntity);
            int num = await this._context.SaveChangesAsync(new CancellationToken());
            return true;
        }

        public async System.Threading.Tasks.Task<bool> UploadIcon(
          Guid currencyPairId,
          string fileUrl)
        {
            CurrencyPairsEntity currencyPairsEntity = await this._context.CurrencyPairs.FindAsync(new object[1]
            {
        (object) currencyPairId
            });
            if (currencyPairsEntity == null)
                return false;
            currencyPairsEntity.Icon = fileUrl;
            this._context.CurrencyPairs.Update(currencyPairsEntity);
            int num = await this._context.SaveChangesAsync(new CancellationToken());
            return true;
        }
    }
}
