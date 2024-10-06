
using Netherite.Domain.Models;
using System;


#nullable enable
namespace Netherite.Domain.Repositories
{
    public interface IMinerRepository
    {
        System.Threading.Tasks.Task<bool> Start(Miner miner, int minerSeconds);

        System.Threading.Tasks.Task<(double remainingTime, bool isFound)> Get(Guid userId);

        System.Threading.Tasks.Task<bool> End(Guid userId);
    }
}
