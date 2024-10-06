
using Netherite.Domain.Models;
using Netherite.Domain.Repositories;
using Netherite.Domain.Services;
using System;
using System.Collections.Generic;


#nullable enable
namespace Netherite.Application.Services
{
    public class CurrencyPairsServices : ICurrencyPairsService
    {
        private readonly ICurrencyPairsRepository _currencyPairsRepository;

        public CurrencyPairsServices(ICurrencyPairsRepository currencyPairsRepository) => this._currencyPairsRepository = currencyPairsRepository;

        public System.Threading.Tasks.Task<Guid> CreateCurrencyPairs(CurrencyPairs currencyPairs) => this._currencyPairsRepository.Create(currencyPairs);

        public System.Threading.Tasks.Task<bool> DeleteCurrencyPairs(Guid currencyPairsId) => this._currencyPairsRepository.Delete(currencyPairsId);

        public System.Threading.Tasks.Task<List<CurrencyPairs>> GetCurrencyPairs() => this._currencyPairsRepository.GetCurrencyPairs();

        public System.Threading.Tasks.Task<bool> UpdateCurrencyPairs(
          Guid currencyPairsId,
          CurrencyPairs currencyPairs)
        {
            return this._currencyPairsRepository.Update(currencyPairsId, currencyPairs);
        }

        public async System.Threading.Tasks.Task<bool> UploadIcon(
          Guid currencyPairId,
          string fileUrl)
        {
            bool flag = await this._currencyPairsRepository.UploadIcon(currencyPairId, fileUrl);
            return flag;
        }
    }
}
