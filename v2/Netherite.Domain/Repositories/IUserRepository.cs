using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Netherite.Domain.Models;
using Task = Netherite.Domain.Models.Task;

namespace Netherite.Domain.Repositories
{
	public interface IUserRepository
	{
		Task<User> Get(Guid userId);
		
		Task<User> GetByWallet(string wallet);

		Task<Guid> Register(User user);

		Task<User> Update(Guid userId, User updatedUser);

		Task<List<Task>> GetAvailableTasks(Guid userId);

		Task<Guid> Complete(Guid userId, Guid taskId);

		Task<List<object>> GetReferals(Guid userId);
	}
}
