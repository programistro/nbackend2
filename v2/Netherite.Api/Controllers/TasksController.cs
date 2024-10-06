
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Netherite.API.Contracts;
using Netherite.Domain.Services;

#nullable enable
namespace Netherite.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITasksServices _tasksServices;

        public TasksController(ITasksServices tasksServices)
        {
            _tasksServices = tasksServices;
        }

        /// <summary>
        /// Получение списка заданий
        /// </summary>
        [AllowAnonymous]
		[HttpGet]
        public async System.Threading.Tasks.Task<ActionResult<List<TasksResponse>>> GetTasks()
        {
            try
            {
                var tasks = await this._tasksServices.GetAllTasks();
                IEnumerable<TasksResponse> response = tasks.Select<Netherite.Domain.Models.Task, TasksResponse>((Func<Netherite.Domain.Models.Task, TasksResponse>)(t => new TasksResponse(t.Id, t.Title, t.Description, t.Icon, t.Reward, t.Status, t.Link)));
                return this.Ok((object)response);
            }
            catch (Exception ex)
            {
                return this.BadRequest((object)ex.Message);
            }
        }

		/// <summary>
		/// Создание задания
		/// </summary>
		/// <param name="request">Запрос на создание задания содержит заголовок, описание, иконку, награду.</param>
		[HttpPost]
        public async System.Threading.Tasks.Task<ActionResult<Guid>> CreateTask(
          [FromBody] TasksRequest request)
        {
            try
            {
                (Domain.Models.Task task2, string Error2) = Domain.Models.Task.Create(Guid.NewGuid(), request.Title, request.Description, request.Icon, request.Reward, request.Status, request.Link);
                if (!string.IsNullOrEmpty(Error2))
                    return this.NotFound((object)Error2);
                Guid guid = await this._tasksServices.CreateTask(task2);
                Guid taskId = guid;
                guid = new Guid();
                return this.Ok((object)taskId);
            }
            catch (Exception ex)
            {
                return this.BadRequest((object)ex.Message);
            }
        }

		/// <summary>
		/// Обновление данных задания
		/// </summary>
		/// <param name="taskId">ID задания</param>
		/// <param name="request">Запрос на обновление задания содержит заголовок, описание, иконку, награду.</param>
		[HttpPut("/Tasks/{taskId}")]
        public async System.Threading.Tasks.Task<ActionResult<bool>> UpdateTask(
          Guid taskId,
          [FromBody] TasksRequest request)
        {
            try
            {
                (Domain.Models.Task task2, string Error2) = Domain.Models.Task.Create(taskId, request.Title, request.Description, request.Icon, request.Reward, request.Status, request.Link);
                if (!string.IsNullOrEmpty(Error2))
                    return this.BadRequest((object)Error2);
                bool result = await this._tasksServices.UpdateTask(taskId, task2);
                return result ? this.Ok((object)result) : this.NotFound((object)"Task not found or could not be updated.");
            }
            catch (Exception ex)
            {
                return this.BadRequest((object)ex.Message);
            }
        }

		/// <summary>Удаление задания</summary>
		/// <param name="taskId">ID задания</param>
		[HttpDelete("/Tasks/{taskId}")]
        public async System.Threading.Tasks.Task<ActionResult<bool>> DeleteTask(
          Guid taskId)
        {
            try
            {
                bool result = await this._tasksServices.DeleteTask(taskId);
                return result ? this.Ok((object)result) : (ActionResult)this.NotFound((object)"Task not found or could not be updated.");
            }
            catch (Exception ex)
            {
                return this.BadRequest((object)ex.Message);
            }
        }
    }
}
