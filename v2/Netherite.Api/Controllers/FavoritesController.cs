
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
    public class FavoritesController : ControllerBase
    {
        private readonly IFavoritesService _favoritesService;

        public FavoritesController(IFavoritesService favoritesService)
        {            
            this._favoritesService = favoritesService;
        }

        //// <summary>
        /// Получение списка избранных пользователя
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        [AllowAnonymous]
		[HttpGet("{userId}")]
        public async System.Threading.Tasks.Task<ActionResult<List<CurrencyPairsResponse>>> GetFavoritesCurrencyPairs(
          Guid userId)
        {
            try
            {
                List<CurrencyPairs> favorites = await this._favoritesService.GetFavorites(userId);
                IEnumerable<CurrencyPairsResponse> response = favorites.Select<CurrencyPairs, CurrencyPairsResponse>((Func<CurrencyPairs, CurrencyPairsResponse>)(t => new CurrencyPairsResponse(t.Id, t.Symbol, t.SymbolTwo, t.Icon)));
                return (ActionResult)this.Ok((object)response);
            }
            catch (Exception ex)
            {
                return this.BadRequest((object)ex.Message);
            }
        }

		/// <summary>
		/// Создание избранной валютной пары
		/// </summary>
		/// <param name="userId">ID пользователя</param>
		/// <param name="currencyPairsId">ID валютной пары</param>
		[HttpPost("add/{userId}/{currencyPairsId}")]
        public async System.Threading.Tasks.Task<ActionResult<Guid>> CreateFavorite(
          Guid userId,
          Guid currencyPairsId)
        {
            try
            {
                Guid guid = await this._favoritesService.CreateFavorites(userId, currencyPairsId);
                Guid favoritesId = guid;
                guid = new Guid();
                return this.Ok((object)favoritesId);
            }
            catch (Exception ex)
            {
                return this.BadRequest((object)ex.Message);
            }
        }

		/// <summary>
		/// Удаление валютной пары из избранного
		/// </summary>
		/// <param name="userId">ID пользователя</param>
		/// <param name="currencyPairsId">ID валютной пары</param>
		[HttpDelete("remove/{userId}/{currencyPairsId}")]
        public async System.Threading.Tasks.Task<ActionResult<bool>> DeleteFavorite(
          Guid userId,
          Guid currencyPairsId)
        {
            try
            {
                bool result = await this._favoritesService.DeleteFavorites(userId, currencyPairsId);
                return result ? this.Ok((object)result) : this.NotFound((object)"Favorites not found or could not be updated.");
            }
            catch (Exception ex)
            {
                return this.BadRequest((object)ex.Message);
            }
        }
    }
}
