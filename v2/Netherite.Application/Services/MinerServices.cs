
using Netherite.Domain.Models;
using Netherite.Domain.Repositories;
using Netherite.Domain.Services;
using System;


#nullable enable
namespace Netherite.Application.Services
{
    public class MinerServices : IMinerServices
    {
        private readonly IMinerRepository _minerRepository;

        public MinerServices(IMinerRepository minerRepository) => this._minerRepository = minerRepository;

        public async System.Threading.Tasks.Task<bool> EndMining(Guid userId)
        {
            bool flag = await this._minerRepository.End(userId);
            return flag;
        }

        public async System.Threading.Tasks.Task<(double remainingTime, bool isFound)> GetCurrentTime(
          Guid userId)
        {
            (double, bool) valueTuple = await this._minerRepository.Get(userId);
            return valueTuple;
        }

        public async System.Threading.Tasks.Task<bool> StartMining(Miner miner, int minerSeconds)
        {
            bool flag = await this._minerRepository.Start(miner, minerSeconds);
            return flag;
        }
    }
}
