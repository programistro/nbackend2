
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Netherite.API.Contracts;
using Netherite.Domain.Models;
using Netherite.Domain.Services;

#nullable enable
namespace Netherite.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class IntervalController : ControllerBase
    {
        private readonly IIntervalServices _intervalServices;

        public IntervalController(IIntervalServices intervalServices)
        {
            this._intervalServices = intervalServices;
        }

        /// <summary>
        /// Получение списка интервалов валютной пары
        /// </summary>
        /// <param name="pairsId">ID валютной пары</param>
        [AllowAnonymous]
		[HttpGet("{pairsId}")]
        public async System.Threading.Tasks.Task<ActionResult<List<IntervalsResponse>>> GetIntervalsByPairsId(Guid pairsId)
        {
            try
            {
                var intervals = await this._intervalServices.GetIntervalByPairsId(pairsId);
                if (intervals == null)
                    return this.NotFound();
                var response = intervals.Select<Interval, IntervalsResponse>((Func<Interval, IntervalsResponse>)(t => new IntervalsResponse(t.Id, t.Time, t.InterestRate)));
                return this.Ok((object)response);
            }
            catch (Exception ex)
            {
                return this.BadRequest((object)ex.Message);
            }
        }

		/// <summary>
		/// Создание интервала
		/// </summary>
		/// <param name="pairsId">ID валютной пары для привязки интервала</param>
		/// <param name="request">Запрос на создание интервала содержит время интервала и величину ставки.</param>
		[HttpPost("{pairsId}")]
        public async System.Threading.Tasks.Task<ActionResult<Guid>> CreateInterval(Guid pairsId, [FromBody] IntervalsRequest request)
        {
            try
            {
                Interval interval = Interval.Create(Guid.NewGuid(), request.Time, request.InterestRate);
                Guid guid = await this._intervalServices.CreateInterval(interval, pairsId);
                Guid intervalId = guid;
                guid = new Guid();
                return !(intervalId == Guid.Empty) ? this.Ok((object)intervalId) : this.NotFound((object)"Валютная пара для привязки интервала не найдена");
            }
            catch (Exception ex)
            {
                return this.BadRequest((object)ex.Message);
            }
        }

		/// <summary>
		/// Обновление данных интервала
		/// </summary>
		/// <param name="intervalId">ID интервала</param>
		/// <param name="request">Запрос на обновление интервала содержит время интервала.</param>
		[HttpPut("{intervalId}")]
        public async System.Threading.Tasks.Task<ActionResult<bool>> UpdateInterval(Guid intervalId, [FromBody] IntervalsRequest request)
        {
            try
            {
                Interval interval = Interval.Create(intervalId, request.Time, request.InterestRate);
                bool result = await this._intervalServices.UpdateInterval(intervalId, interval);
                return result ? this.Ok((object)result) : this.NotFound((object)"Interval not found or could not be updated.");
            }
            catch (Exception ex)
            {
                return this.BadRequest((object)ex.Message);
            }
        }

		/// <summary>
		/// Удаление интервала
		/// </summary>
		/// <param name="intervalId">ID интервала</param>
		[HttpDelete("{intervalId}")]
        public async System.Threading.Tasks.Task<ActionResult<bool>> DeleteInterval(Guid intervalId)
        {
            try
            {
                bool result = await this._intervalServices.DeleteInterval(intervalId);
                return result ? this.Ok((object)result) : this.NotFound((object)"Interval not found or could not be updated.");
            }
            catch (Exception ex)
            {
                return this.BadRequest((object)ex.Message);
            }
        }
    }
}
