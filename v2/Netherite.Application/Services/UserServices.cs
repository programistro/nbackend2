
using Netherite.Domain.Models;
using Netherite.Domain.Repositories;
using Netherite.Domain.Services;


#nullable enable
namespace Netherite.Application.Services
{
    public class UserServices : IUserServices
    {
        private readonly IUserRepository _userRepository;

        public UserServices(IUserRepository userRepository) => this._userRepository = userRepository;

        public async System.Threading.Tasks.Task<User> GetUser(Guid userId)
        {
            User user = await this._userRepository.Get(userId);
            return user;
        }

        public async System.Threading.Tasks.Task<User?> GetUserByWallet(string wallet)
        {
            User byWallet = await this._userRepository.GetByWallet(wallet);
            return byWallet;
        }

        public async System.Threading.Tasks.Task<Guid> RegisterUser(User user)
        {
            Guid guid = await this._userRepository.Register(user);
            return guid;
        }

        public async System.Threading.Tasks.Task<User> UpdateUser(Guid userId, User updatedUser)
        {
            User user = await this._userRepository.Update(userId, updatedUser);
            return user;
        }

        public async System.Threading.Tasks.Task<List<Netherite.Domain.Models.Task>> GetAvailableTasks(
          Guid userId)
        {
            List<Netherite.Domain.Models.Task> availableTasks = await this._userRepository.GetAvailableTasks(userId);
            return availableTasks;
        }

        public async System.Threading.Tasks.Task<Guid> CompleteTask(Guid userId, Guid taskId)
        {
            Guid guid = await this._userRepository.Complete(userId, taskId);
            return guid;
        }

        public async System.Threading.Tasks.Task<List<object>> GetReferals(Guid userId)
        {
            List<object> referals = await this._userRepository.GetReferals(userId);
            return referals;
        }
    }
}
