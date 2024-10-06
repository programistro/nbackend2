using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Netherite.Domain.Models;
using Task = Netherite.Domain.Models.Task;

namespace Netherite.Domain.Services
{
	public interface IUserServices
	{
		Task<User> GetUser(Guid userId);
		
		Task<User> GetUserByWallet(string wallet);

		Task<Guid> RegisterUser(User user);

		Task<User> UpdateUser(Guid userId, User updatedUser);

		Task<List<Task>> GetAvailableTasks(Guid userId);

		Task<Guid> CompleteTask(Guid userId, Guid taskId);

		Task<List<object>> GetReferals(Guid userId);
	}
}
