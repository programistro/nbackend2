using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Netherite.Domain.Models;
using Task = Netherite.Domain.Models.Task;

namespace Netherite.Domain.Repositories
{
	public interface ITasksRepository
	{
		Task<List<Task>> Get();

		Task<Guid> Create(Task task);

		Task<bool> Delete(Guid taskId);

		Task<bool> Update(Guid taskId, Task task);
	}
}
