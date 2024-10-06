using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Netherite.Domain.Models;
using Netherite.Domain.Repositories;
using Netherite.Domain.Services;

namespace Netherite.Application.Services
{
	public class IntervalServices : IIntervalServices
	{
		public IntervalServices(IIntervalsRepository intervalRepository)
		{
			this._intervalRepository = intervalRepository;
		}

		public async Task<Guid> CreateInterval(Interval interval, Guid pairsId)
		{
			return await this._intervalRepository.Create(interval, pairsId);
		}

		public async Task<bool> DeleteInterval(Guid intervalId)
		{
			return await this._intervalRepository.Delete(intervalId);
		}

		public async Task<List<Interval>> GetIntervalByPairsId(Guid pairsId)
		{
			return await this._intervalRepository.GetByPairsId(pairsId);
		}

		public async Task<Interval> GetIntervalById(Guid intervalId)
		{
			return await this._intervalRepository.GetById(intervalId);
		}

		public async Task<bool> UpdateInterval(Guid intervalId, Interval interval)
		{
			return await this._intervalRepository.Update(intervalId, interval);
		}

		private readonly IIntervalsRepository _intervalRepository;
	}
}
