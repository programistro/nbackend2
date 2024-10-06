using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Netherite.Domain.Models;

namespace Netherite.Domain.Repositories
{
	public interface IIntervalsRepository
	{
		Task<List<Interval>> GetByPairsId(Guid currencyPairsId);

		Task<Guid> Create(Interval interval, Guid pairsId);

		Task<bool> Delete(Guid intervalId);

		Task<bool> Update(Guid intervalId, Interval interval);

		Task<Interval> GetById(Guid intervalId);
	}
}
