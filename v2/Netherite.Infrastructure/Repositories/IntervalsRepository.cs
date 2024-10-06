
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Netherite.Domain.Models;
using Netherite.Domain.Repositories;
using Netherite.Infrastructure.Entities;
using System.Linq.Expressions;

#nullable enable
namespace Netherite.Infrastructure.Repositories
{
	public class IntervalsRepository : IIntervalsRepository
    {
        private readonly NetheriteDbContext _context;

        public IntervalsRepository(NetheriteDbContext context) => this._context = context;

        public async System.Threading.Tasks.Task<List<Interval>> GetByPairsId(
          Guid currencyPairsId)
        {
            List<CurrencyPairsIntervalEntity> currencyPairsIntervalsEntities = await (System.Threading.Tasks.Task<List<CurrencyPairsIntervalEntity>>)EntityFrameworkQueryableExtensions.ToListAsync<CurrencyPairsIntervalEntity>(((IQueryable<CurrencyPairsIntervalEntity>)EntityFrameworkQueryableExtensions.AsNoTracking<CurrencyPairsIntervalEntity>(this._context.CurrencyPairsIntervals)).Where<CurrencyPairsIntervalEntity>((Expression<Func<CurrencyPairsIntervalEntity, bool>>)(cpi => cpi.CurrencyPairsId == currencyPairsId)), new CancellationToken());
            List<Interval> intervals = new List<Interval>();
            foreach (CurrencyPairsIntervalEntity pairsIntervalEntity in currencyPairsIntervalsEntities)
            {
                CurrencyPairsIntervalEntity entity = pairsIntervalEntity;

                IntervalEntity interval = await (System.Threading.Tasks.Task<IntervalEntity>)EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<IntervalEntity>(((IQueryable<IntervalEntity>)EntityFrameworkQueryableExtensions.AsNoTracking<IntervalEntity>(this._context.Intervals)).Where<IntervalEntity>((Expression<Func<IntervalEntity, bool>>)(i => i.Id == entity.IntervalId)), new CancellationToken());
                
                if (interval != null)
                    intervals.Add(Interval.Create(interval.Id, interval.Time, interval.InterestRate));

                interval = (IntervalEntity)null;
            }
            List<Interval> intervalList = intervals;
            currencyPairsIntervalsEntities = (List<CurrencyPairsIntervalEntity>)null;
            intervals = (List<Interval>)null;
            return intervalList;
        }

        public async System.Threading.Tasks.Task<Guid> Create(Interval interval, Guid pairsId)
        {
            IntervalEntity intervalEntity = new IntervalEntity()
            {
                Id = interval.Id,
                Time = interval.Time,
                InterestRate = interval.InterestRate,
            };
            CurrencyPairsEntity currencyPairEntity = await (System.Threading.Tasks.Task<CurrencyPairsEntity>)EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<CurrencyPairsEntity>(this._context.CurrencyPairs, (cp => cp.Id == pairsId), new CancellationToken());
            if (currencyPairEntity == null)
                return Guid.Empty;
            CurrencyPairsIntervalEntity currencyPairsIntervalsEntities = new CurrencyPairsIntervalEntity()
            {
                Id = Guid.NewGuid(),
                CurrencyPairsId = currencyPairEntity.Id,
                CurrencyPairs = currencyPairEntity,
                IntervalId = intervalEntity.Id,
                Interval = intervalEntity,                
            };
            EntityEntry<IntervalEntity> entityEntry1 = await this._context.Intervals.AddAsync(intervalEntity, new CancellationToken());
            EntityEntry<CurrencyPairsIntervalEntity> entityEntry2 = await this._context.CurrencyPairsIntervals.AddAsync(currencyPairsIntervalsEntities, new CancellationToken());
            int num = await this._context.SaveChangesAsync(new CancellationToken());
            return intervalEntity.Id;
        }

        public async System.Threading.Tasks.Task<bool> Delete(Guid intervalId)
        {
            IntervalEntity intervalEntity = await this._context.Intervals.FindAsync(new object[1] { (object) intervalId });

            if (intervalEntity == null) return false;

            this._context.Intervals.Remove(intervalEntity);
            int num = await this._context.SaveChangesAsync(new CancellationToken());
            return true;
        }

        public async System.Threading.Tasks.Task<bool> Update(Guid intervalId, Interval interval)
        {
            IntervalEntity intervalEntity = await this._context.Intervals.FindAsync(new object[1] { (object) intervalId });

            if (intervalEntity == null)
                return false;

            intervalEntity.Time = interval.Time;
			intervalEntity.InterestRate = interval.InterestRate;

			this._context.Intervals.Update(intervalEntity);
            int num = await this._context.SaveChangesAsync(new CancellationToken());
            return true;
        }

		public async Task<Interval> GetById(Guid intervalId)
		{
			IntervalEntity intervalEntity = await this._context.Intervals.FindAsync(new object[] { intervalId });

            return Interval.Create(intervalEntity.Id, intervalEntity.Time, intervalEntity.InterestRate); 
		}
	}
}
