using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Netherite.Domain.Models;

namespace Netherite.Domain.Services
{
	public interface IMinerServices
	{
		Task<bool> StartMining(Miner miner, int minerSeconds);
		
		Task<(double remainingTime, bool isFound)> GetCurrentTime(Guid userId);

		Task<bool> EndMining(Guid userId);
	}
}
