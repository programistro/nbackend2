
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Netherite.Domain.Models;
using Netherite.Domain.Repositories;
using Netherite.Domain.Services;
using Netherite.Infrastructure.Entities;
using System;
using System.Linq.Expressions;


#nullable enable
namespace Netherite.Infrastructure.Repositories
{
    public class MinerRepository : IMinerRepository
    {
        private readonly NetheriteDbContext _context;
        private readonly IReferalBonusesServices _referalBonusesServices;

        public MinerRepository(
          NetheriteDbContext context,
          IReferalBonusesServices referalBonusesServices)
        {
            this._context = context;
            this._referalBonusesServices = referalBonusesServices;
        }

        public async System.Threading.Tasks.Task<bool> Start(Miner miner, int minerSeconds)
        {
            MinerEntity existingMiner = await this._context.Miners.FirstOrDefaultAsync<MinerEntity>((Expression<Func<MinerEntity, bool>>)(m => m.UserId == miner.UserId));
            if (existingMiner != null)
                return false;
            miner.StartTime = DateTime.UtcNow;
            miner.EndTime = miner.StartTime.AddSeconds((double)minerSeconds);
            MinerEntity minerEntity = new MinerEntity()
            {
                Id = miner.Id,
                UserId = miner.UserId,
                Reward = miner.Reward,
                StartTime = miner.StartTime,
                EndTime = miner.EndTime
            };
            EntityEntry<MinerEntity> entityEntry = await this._context.Miners.AddAsync(minerEntity);
            int num = await this._context.SaveChangesAsync();
            return true;
        }

        public async System.Threading.Tasks.Task<(double remainingTime, bool isFound)> Get(
          Guid userId)
        {
            MinerEntity miner = await this._context.Miners.FirstOrDefaultAsync<MinerEntity>((Expression<Func<MinerEntity, bool>>)(m => m.UserId == userId));
            if (miner == null)
                return (0.0, false);
            double remainingTime = (miner.EndTime - DateTime.UtcNow).TotalSeconds;
            return (remainingTime > 0.0 ? remainingTime : 0.0, true);
        }

        public async System.Threading.Tasks.Task<bool> End(Guid userId)
        {
            MinerEntity miner = await this._context.Miners.FirstOrDefaultAsync<MinerEntity>((Expression<Func<MinerEntity, bool>>)(m => m.UserId == userId));
            if (miner == null)
                return false;
            double remainingTime = (miner.EndTime - DateTime.UtcNow).TotalSeconds;
            if (remainingTime > 0.0)
                return false;
            int reward = miner.Reward;
            UserEntity user = await this._context.Users.FirstOrDefaultAsync<UserEntity>((Expression<Func<UserEntity, bool>>)(u => u.Id == userId));
            if (user == null)
                return false;
            user.Balance += (Decimal)reward;
            UserEntity referrer = await this._context.Users.FirstOrDefaultAsync<UserEntity>((Expression<Func<UserEntity, bool>>)(u => (Guid?)u.Id == user.InvitedId));
            UserEntity referrersReferrer = (UserEntity)null;
            if (referrer != null)
                referrersReferrer = await this._context.Users.FirstOrDefaultAsync<UserEntity>((Expression<Func<UserEntity, bool>>)(u => (Guid?)u.Id == referrer.InvitedId));
            (int, int) valueTuple1 = await this._referalBonusesServices.Execute(user.IsPremium, reward);
            (int, int) valueTuple2 = valueTuple1;
            int referrerReward = valueTuple2.Item1;
            int referrersReferrerReward = valueTuple2.Item2;
            //valueTuple1 = ();
            //valueTuple2 = ();
            if (referrer != null)
            {
                referrer.Balance += (Decimal)referrerReward;
                user.Profit += referrerReward;
                if (referrersReferrer != null)
                {
                    referrersReferrer.Balance += (Decimal)referrersReferrerReward;
                    referrer.Profit += referrersReferrerReward;
                }
            }
            this._context.Miners.Remove(miner);
            int num = await this._context.SaveChangesAsync();
            return true;
        }
    }
}
