using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Netherite.Domain.Models;
using Task = Netherite.Domain.Models.Task;

namespace Netherite.Domain.Services
{
	public interface ITasksServices
	{
		Task<List<Task>> GetAllTasks();

		Task<Guid> CreateTask(Task task);

		Task<bool> DeleteTask(Guid taskId);

		Task<bool> UpdateTask(Guid taskId, Task task);
	}
}
