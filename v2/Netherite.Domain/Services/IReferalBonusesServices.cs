using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Netherite.Domain.Services
{
	public interface IReferalBonusesServices
	{    
        Task<(int referrerReward, int referrersReferrerReward)> Execute(bool isPremium, int reward);
	}
}
