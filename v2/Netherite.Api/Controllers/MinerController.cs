
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Netherite.Domain.Models;
using Netherite.Domain.Services;
using System;


#nullable enable
namespace Netherite.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class MinerController : ControllerBase
    {
        private readonly IMinerServices _minerServices;

        public MinerController(IMinerServices minerServices)
        {
            this._minerServices = minerServices;
        }

		/// <summary>
		/// Получает оставшееся время майнинга определенного пользователя
		/// </summary>
		/// <param name="userId">ID пользователя</param>
		/// <returns>Оставшееся время в секундах</returns>
        [AllowAnonymous]        
		[HttpGet("time/{userId}")]
        public async System.Threading.Tasks.Task<ActionResult<double>> GetTimeOfMining(
          Guid userId)
        {
            try
            {
                (double, bool) valueTuple1 = await this._minerServices.GetCurrentTime(userId);
                (double, bool) valueTuple2 = valueTuple1;
                double remainingTime = valueTuple2.Item1;
                bool isFound = valueTuple2.Item2;
                //valueTuple1 = ();
                //valueTuple2 = ();
                return isFound ? this.Ok((object)remainingTime) : this.NotFound((object)"Майнинг ещё не начат.");
            }
            catch (Exception ex)
            {
                return this.BadRequest((object)ex.Message);
            }
        }

		/// <summary>
		/// Запускает майнинг для опредленного пользователя
		/// </summary>
		/// <param name="userId">ID пользователя</param>
		/// <returns>Булево значение, указывающее, успешно ли начался майнинг.</returns>
		[HttpPost("start/{userId}")]
        public async System.Threading.Tasks.Task<ActionResult<bool>> StartMining(
          Guid userId)
        {
            try
            {
                Miner miner = Miner.Create(Guid.NewGuid(), userId, 100, DateTime.UtcNow, DateTime.UtcNow.AddSeconds(40.0));
                bool result = await this._minerServices.StartMining(miner, 40);
                return result ? this.Ok((object)result) : this.BadRequest((object)"Майнинг уже начат.");
            }
            catch (Exception ex)
            {
                return (ActionResult)this.BadRequest((object)ex.Message);
            }
        }

		/// <summary>
		/// Заканчивет майнинг определенного пользователя.
		/// </summary>
		/// <param name="userId">ID пользователя</param>
		/// <returns>Булево значение, указывающее, успешно ли закончен майнинг</returns>
		[HttpDelete("end/{userId}")]
        public async System.Threading.Tasks.Task<ActionResult<bool>> EndMining(
          Guid userId)
        {
            try
            {
                bool result = await this._minerServices.EndMining(userId);
                return result ? this.Ok((object)result) : this.BadRequest((object)"Не удалось завершить майнинг. Либо майнинг не найден, либо время не равно нулю.");
            }
            catch (Exception ex)
            {
                return this.BadRequest((object)ex.Message);
            }
        }
    }
}
