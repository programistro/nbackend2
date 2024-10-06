
using Netherite.Domain.Repositories;
using Netherite.Domain.Services;
using System;
using System.Collections.Generic;


#nullable enable
namespace Netherite.Application.Services
{
    public class TasksServices : ITasksServices
    {
        private readonly ITasksRepository _taskRepository;

        public TasksServices(ITasksRepository tasksRepository) => this._taskRepository = tasksRepository;

        public async System.Threading.Tasks.Task<List<Netherite.Domain.Models.Task>> GetAllTasks()
        {
            List<Netherite.Domain.Models.Task> taskList = await this._taskRepository.Get();
            return taskList;
        }

        public async System.Threading.Tasks.Task<Guid> CreateTask(Netherite.Domain.Models.Task task)
        {
            Guid guid = await this._taskRepository.Create(task);
            return guid;
        }

        public async System.Threading.Tasks.Task<bool> DeleteTask(Guid taskId)
        {
            bool flag = await this._taskRepository.Delete(taskId);
            return flag;
        }

        public async System.Threading.Tasks.Task<bool> UpdateTask(Guid taskId, Netherite.Domain.Models.Task task)
        {
            bool flag = await this._taskRepository.Update(taskId, task);
            return flag;
        }
    }
}
