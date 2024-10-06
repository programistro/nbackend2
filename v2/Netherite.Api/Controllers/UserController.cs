
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Netherite.API.Contracts;
using Netherite.Domain.Services;
using User = Netherite.Domain.Models.User;

#nullable enable
namespace Netherite.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userServices;

        public UserController(IUserServices userServices)
        {
            this._userServices = userServices;
        }

        /// <summary>
        /// Получение определенного пользователя
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        [AllowAnonymous]
        [HttpGet("{userId}")]
        public async System.Threading.Tasks.Task<ActionResult<UserResponse>> GetUser(
            Guid userId)
        {
            try
            {
                User user = await this._userServices.GetUser(userId);
                if (user == null)
                    return this.NotFound((object)"Пользователь не найден");
                UserResponse response = new UserResponse(user.Id, user.Balance, user.Location, user.InvitedId,
                    user.IsPremium, user.TelegramId, user.TelegramName, user.Wallet, user.Profit);
                return this.Ok((object)response);
            }
            catch (Exception ex)
            {
                return this.BadRequest((object)ex.Message);
            }
        }

        /// <summary>
        /// Получение определенного пользователя по номеру кошелька
        /// </summary>
        /// <param name="wallet">Номер кошелька</param>
        [AllowAnonymous]
        [HttpGet("by-wallet/{wallet}")]
        public async System.Threading.Tasks.Task<ActionResult<UserResponse>> GetUserByWallet(
            string wallet)
        {
            try
            {
                User user = await this._userServices.GetUserByWallet(wallet);
                if (user == null)
                    return this.NotFound((object)"Пользователь не найден");
                UserResponse response = new UserResponse(user.Id, user.Balance, user.Location, user.InvitedId,
                    user.IsPremium, user.TelegramId, user.TelegramName, user.Wallet, user.Profit);
                return this.Ok((object)response);
            }
            catch (Exception ex)
            {
                return this.BadRequest((object)ex.Message);
            }
        }

        /// <summary>
        /// Получение доступных пользователю заданий
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        [AllowAnonymous]
        [HttpGet("tasks/{userId}")]
        public async System.Threading.Tasks.Task<ActionResult<List<Netherite.Domain.Models.Task>>> GetAvailableTasks(
            Guid userId)
        {
            try
            {
                List<Netherite.Domain.Models.Task> tasks = await this._userServices.GetAvailableTasks(userId);
                return tasks != null ? this.Ok((object)tasks) : NotFound((object)"Не найдено доступных заданий");
            }
            catch (Exception ex)
            {
                return this.BadRequest((object)ex.Message);
            }
        }

        /// <summary>
        /// Получение рефералов пользователя
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        [AllowAnonymous]
        [HttpGet("referals/{userId}")]
        public async System.Threading.Tasks.Task<ActionResult<Guid>> GetReferals(
            Guid userId)
        {
            try
            {
                List<object> referals = await this._userServices.GetReferals(userId);
                return this.Ok((object)referals);
            }
            catch (Exception ex)
            {
                return this.BadRequest((object)ex.Message);
            }
        }

        /// <summary>
        /// Регистрация пользователя
        /// </summary>
        /// <param name="request">Запрос на создание пользователя содержит баланс, локацию, ID пригласившего, премиум, телеграм ID, телеграм имя, номер кошелька.</param>
        [HttpPost("register")]
        public async System.Threading.Tasks.Task<ActionResult<Guid>> RegisterUser(
            [FromBody] UserRequest request)
        {
            try
            {
                (User user2, string Error2) = Netherite.Domain.Models.User.Create(Guid.NewGuid(), 0M, request.Location,
                    request.InvitedId, request.IsPremium, request.TelegramId, request.TelegramName, request.Wallet, 0);
                if (!string.IsNullOrEmpty(Error2))
                    return this.BadRequest((object)Error2);
                Guid guid = await this._userServices.RegisterUser(user2);
                Guid userId = guid;
                guid = new Guid();
                return this.Ok((object)userId);
            }
            catch (Exception ex)
            {
                return this.BadRequest((object)ex.Message);
            }
        }

        /// <summary>
        /// Выполнение задания
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        /// <param name="taskId">ID задания</param>
        [HttpPost("complete/{userId}/{taskId}")]
        public async System.Threading.Tasks.Task<ActionResult<Guid>> CompleteTask(
            Guid userId,
            Guid taskId)
        {
            try
            {
                Guid guid = await this._userServices.CompleteTask(userId, taskId);
                Guid completedTaskId = guid;
                guid = new Guid();
                return this.Ok((object)completedTaskId);
            }
            catch (Exception ex)
            {
                return this.BadRequest((object)ex.Message);

            }

            /// <summary>
            /// Обновление пользователя
            /// </summary>
            /// <param name="userId">ID пользователя</param>
            /// <param name="updatedUserRequest">Запрос на обновление пользователя содержит локацию, ID пригласившего, премиум, телеграм ID, телеграм имя, номер кошелька.</param>
          
        }
        [HttpPut("update/{userId}")]
        public async System.Threading.Tasks.Task<ActionResult<UserResponse>> UpdateUser(
            Guid userId,
            [FromBody] UserRequest updatedUserRequest)
        {
            try
            {
                User existingUser = await this._userServices.GetUser(userId);
                if (existingUser == null)
                    return this.NotFound((object)"Пользователь не найден");
                (User user3, string Error2) = Netherite.Domain.Models.User.Create(userId, existingUser.Balance,
                    updatedUserRequest.Location, updatedUserRequest.InvitedId, updatedUserRequest.IsPremium,
                    updatedUserRequest.TelegramId, updatedUserRequest.TelegramName, updatedUserRequest.Wallet,
                    existingUser.Profit);
                if (!string.IsNullOrEmpty(Error2))
                    return this.BadRequest((object)Error2);
                User user = await this._userServices.UpdateUser(userId, user3);
                UserResponse response = new UserResponse(user.Id, user.Balance, user.Location, user.InvitedId,
                    user.IsPremium, user.TelegramId, user.TelegramName, user.Wallet, user.Profit);
                return this.Ok((object)response);
            }
            catch (Exception ex)
            {
                return this.BadRequest((object)ex.Message);
            }
        }
    }
}
