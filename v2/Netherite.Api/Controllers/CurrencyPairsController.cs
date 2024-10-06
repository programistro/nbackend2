
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Netherite.Api.Contracts;
using Netherite.API.Contracts;
using Netherite.Domain.Models;
using Netherite.Domain.Services;
using Netherite.Infrastructure;
using Newtonsoft.Json.Linq;

#nullable enable
namespace Netherite.API.Controllers
{
	[ApiController]
    [Authorize]
    [Route("[controller]")]
	public class CurrencyPairsController : ControllerBase
	{
		private readonly ICurrencyPairsService _currencyPairsService;

        private readonly NetheriteDbContext _context;

        public CurrencyPairsController(ICurrencyPairsService currencyPairsService, NetheriteDbContext context)
		{
			this._currencyPairsService = currencyPairsService;
			_context = context;
		}

		/// <summary>
		/// Получение списка валютных пар
		/// </summary>
		[AllowAnonymous]
		[HttpGet]
		public async System.Threading.Tasks.Task<ActionResult<List<CurrencyPairsResponse>>> GetCurrencyPairs()
		{
			try
			{
				
				List<CurrencyPairs> currencyPairs = await this._currencyPairsService.GetCurrencyPairs();
				if (currencyPairs == null)
					return this.NotFound();

				IEnumerable<CurrencyPairsResponse> response = currencyPairs.Select<CurrencyPairs, CurrencyPairsResponse>((Func<CurrencyPairs, CurrencyPairsResponse>)(cp => new CurrencyPairsResponse(cp.Id, cp.Symbol, cp.SymbolTwo, cp.Icon)));
				return this.Ok((object)response);
			}
			catch (Exception ex)
			{
				return this.BadRequest((object)ex.Message);
			}
		}

		/// <summary>
		/// Создание валютной пары
		/// </summary>
		/// <param name="request">Запрос на создание валютной пары содержит имя валютной пары, процент прибыли валюнтой пары.</param>
		[HttpPost]
		public async System.Threading.Tasks.Task<ActionResult<Guid>> CreateCurrencyPairs([FromForm] CurrencyPairsRequest request)
		{
			try
			{
				string fileUrl = await GetIconUrl(request.Symbol);

				var currencyPair = CurrencyPairs.Create(Guid.NewGuid(), request.Symbol, request.SymbolTwo, fileUrl);

				var currencyPairId = await this._currencyPairsService.CreateCurrencyPairs(currencyPair);

				return this.Ok(currencyPairId);
			}
			catch (Exception ex)
			{
				return this.BadRequest((object)ex.Message);
			}
		}		

		/// <summary>
		/// Обновление валютной пары
		/// </summary>
		/// <param name="currencyPairId">ID валютной пары</param>
		/// <param name="request">Запрос на обновление валютной пары содержит имя валютной пары, процент прибыли валюнтой пары.</param>
		[HttpPut("{currencyPairId}")]
		public async System.Threading.Tasks.Task<ActionResult<bool>> UpdateCurrencyPairs(Guid currencyPairId, [FromBody] CurrencyPairsRequest request)
		{
			try
			{				
				var fileUrl = await GetIconUrl(request.Symbol);
				CurrencyPairs currencyPair = CurrencyPairs.Create(currencyPairId, request.Symbol, request.SymbolTwo, fileUrl);
				bool result = await this._currencyPairsService.UpdateCurrencyPairs(currencyPairId, currencyPair);
				return result ? this.Ok((object)result) : this.NotFound((object)"Валютная пара не найдена или не может быть обновлена");
			}
			catch (Exception ex)
			{
				return this.BadRequest((object)ex.Message);
			}
		}
		
		/// <summary>
		/// Удаление валютной пары
		/// </summary>
		/// <param name="currencyPairId">ID валютной пары</param>
		[HttpDelete("{currencyPairId}")]
		public async System.Threading.Tasks.Task<ActionResult<bool>> DeleteCurrencyPairs(
		  Guid currencyPairId)
		{
			try
			{
				bool result = await this._currencyPairsService.DeleteCurrencyPairs(currencyPairId);
				return result ? this.Ok((object)result) : this.NotFound((object)"Валютная пара не найдена");
			}
			catch (Exception ex)
			{
				return this.BadRequest((object)ex.Message);
			}
		}

		[HttpPost("{currencyPairId}/upload")]
		public async System.Threading.Tasks.Task<ActionResult<bool>> UploadIcon(
		  UploadIconRequest request)
		{
			try
			{
				var fileUrl = await GetIconUrl(request.Symbol);

				bool result = await this._currencyPairsService.UploadIcon(request.CurrencyPairId, fileUrl);
				return result ? this.Ok((object)result) : this.NotFound((object)"Валютная пара не найдена или не может быть обновлена");
			}
			catch (Exception ex)
			{
				return this.BadRequest((object)ex.Message);
			}
		}

        [HttpPut("update-icon-pair")]
        public async Task<IActionResult> UpdateIconCurrencyPairs(Guid id, IFormFile file)
        {
            var pair = _context.CurrencyPairs.FirstOrDefault(x => x.Id == id);

            if (pair == null)
            {
                return NotFound();
            }

            try
            {
                if (file == null || file.Length == 0L)
                    return BadRequest("Файл не выбран или он пустой");

                // Создаем имя файла
                string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                string filePath = Path.Combine("storage", fileName);

                // Создаем директорию, если она не существует
                if (!Directory.Exists("storage"))
                    Directory.CreateDirectory("storage");

                // Сохраняем файл
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Создаем URL файла
                string fileUrl = $"https://{this.Request.Host}/storage/{fileName}";

                pair.Icon = fileUrl;

                _context.CurrencyPairs.Update(pair);
                await _context.SaveChangesAsync();

                return Ok(pair);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        private static List<CoinModel> _coinList;

		private static async Task<List<CoinModel>> GetSymbolList()
		{
			try
			{
				if (_coinList is not null && _coinList.Count > 0)
				{
					return _coinList;
				}

				var coinsList = $"https://api.coingecko.com/api/v3/coins/list";
				
				var httpClient = new HttpClient();
				var response = await httpClient.GetStringAsync(coinsList);

				var data = JArray.Parse(response);

				var result = new List<CoinModel>();
				
				foreach (var item in data)
				{
					result.Add(new CoinModel()
					{
						Id = item["id"].ToString(),
						Symbol = item["symbol"].ToString(),
						Name = item["name"].ToString()
					});
				}

				_coinList = result;

				return result;
			}
			catch (Exception ex)
			{
				throw new Exception($"Ошибка: {ex.Message}");
			}
		}

		/// <summary>
		/// Метод получает путь к файлу иконки символа
		/// </summary>
		/// <param name="symbol"></param>
		/// <returns></returns>
		private static async Task<string> GetIconUrl(string symbol)
		{
			try
			{
				var symbolList = await GetSymbolList();

				var symbolModel = symbolList.FirstOrDefault(x => x.Symbol.Equals(symbol, StringComparison.InvariantCultureIgnoreCase));

				if (symbolModel is null)
				{
					throw new Exception($"Идентификатор символа {symbol} не найден");
				}

				var coinGeckoUrl = $"https://api.coingecko.com/api/v3/coins/{symbolModel.Id}";

				var httpClient = new HttpClient();
				var response = await httpClient.GetStringAsync(coinGeckoUrl);

				var coinData = JObject.Parse(response);

				string fileUrl = coinData["image"]["thumb"].ToString();
				return fileUrl;
			}
			catch (Exception ex)
			{
				throw new Exception($"Иконка символа {symbol} не найдена. Ошибка: {ex.Message}");
			}
		}

        [HttpPost("{currencyPairId}/uploadv2")]
        public async Task<ActionResult<bool>> UploadIcon(Guid currencyPairId, IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0L)
                    return BadRequest("Файл не выбран или он пустой");

                // Создаем имя файла
                string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                string filePath = Path.Combine("storage", fileName);

                // Создаем директорию, если она не существует
                if (!Directory.Exists("storage"))
                    Directory.CreateDirectory("storage");

                // Сохраняем файл
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Создаем URL файла
                string fileUrl = $"{this.Request.Scheme}://{this.Request.Host}/storage/{fileName}";

                // Загружаем иконку
                bool result = await _currencyPairsService.UploadIcon(currencyPairId, fileUrl);

                if (result)
                    return Ok(result);
                else
                    return NotFound("Валютная пара не найдена или не может быть обновлена");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }
    }
}
