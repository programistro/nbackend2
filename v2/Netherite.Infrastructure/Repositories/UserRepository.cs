
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Netherite.Domain.Models;
using Netherite.Domain.Repositories;
using Netherite.Domain.Services;
using Netherite.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;


#nullable enable
namespace Netherite.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly NetheriteDbContext _context;
        private readonly IReferalBonusesServices _referalBonusesServices;

        public UserRepository(
          NetheriteDbContext context,
          IReferalBonusesServices referalBonusesServices)
        {
            this._context = context;
            this._referalBonusesServices = referalBonusesServices;
        }

        public async System.Threading.Tasks.Task<User> Get(Guid userId)
        {
            UserEntity userEntity = await this._context.Users.FindAsync(new object[1]
            {
        (object) userId
            });
            User user = userEntity != null ? this.MapToDomain(userEntity) : throw new Exception("Пользователь не найден");
            userEntity = (UserEntity)null;
            return user;
        }

        public async System.Threading.Tasks.Task<Guid> Register(User user)
        {
            UserEntity existingUser = await (System.Threading.Tasks.Task<UserEntity>)EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<UserEntity>(this._context.Users, u => u.TelegramId == user.TelegramId);
            if (existingUser != null)
                return existingUser.Id;
            UserEntity userEntity = this.MapToEntity(user);
            EntityEntry<UserEntity> entityEntry = await this._context.Users.AddAsync(userEntity, new CancellationToken());
            int num = await this._context.SaveChangesAsync(new CancellationToken());
            return userEntity.Id;
        }

        public async System.Threading.Tasks.Task<User> Update(Guid userId, User updatedUser)
        {
            UserEntity userEntity1 = await this._context.Users.FindAsync(new object[1]
            {
        (object) userId
            });
            UserEntity userEntity2 = userEntity1 ?? throw new Exception("Пользователь не найден");
            userEntity1 = (UserEntity)null;
            userEntity2.Balance = updatedUser.Balance;
            userEntity2.Location = updatedUser.Location;
            userEntity2.InvitedId = updatedUser.InvitedId;
            userEntity2.IsPremium = updatedUser.IsPremium;
            userEntity2.TelegramId = updatedUser.TelegramId;
            userEntity2.TelegramName = updatedUser.TelegramName;
            userEntity2.Wallet = updatedUser.Wallet;
            int num = await this._context.SaveChangesAsync(new CancellationToken());
            User domain = this.MapToDomain(userEntity2);
            userEntity2 = (UserEntity)null;
            return domain;
        }

        public async System.Threading.Tasks.Task<List<Netherite.Domain.Models.Task>> GetAvailableTasks(
          Guid userId)
        {
            List<Guid> userCompletedTaskIds = await (System.Threading.Tasks.Task<List<Guid>>)EntityFrameworkQueryableExtensions.ToListAsync<Guid>(((IQueryable<UserTaskEntity>)EntityFrameworkQueryableExtensions.AsNoTracking<UserTaskEntity>(this._context.UserTasks)).Where<UserTaskEntity>((Expression<Func<UserTaskEntity, bool>>)(ut => ut.UserId == userId)).Select<UserTaskEntity, Guid>((Expression<Func<UserTaskEntity, Guid>>)(ut => ut.TaskId)), new CancellationToken());
            List<TaskEntity> availableTasksEntities = await (System.Threading.Tasks.Task<List<TaskEntity>>)EntityFrameworkQueryableExtensions.ToListAsync<TaskEntity>(this._context.Tasks.Where<TaskEntity>(t => !userCompletedTaskIds.Contains(t.Id)));
            List<Netherite.Domain.Models.Task> availableTasks = new List<Netherite.Domain.Models.Task>();
            foreach (TaskEntity taskEntity in availableTasksEntities)
            {
                TaskEntity entity = taskEntity;
                (Netherite.Domain.Models.Task Task2, string Error2) = Netherite.Domain.Models.Task.Create(entity.Id, entity.Title, entity.Description, entity.Icon, entity.Reward, entity.Status, entity.Link);
                if (string.IsNullOrEmpty(Error2))
                    availableTasks.Add(Task2);
                else
                    Console.WriteLine("Ошибка создания задачи: " + Error2);
                Task2 = (Netherite.Domain.Models.Task)null;
                Error2 = (string)null;
                entity = (TaskEntity)null;
            }
            List<Netherite.Domain.Models.Task> taskList = availableTasks;
            availableTasksEntities = (List<TaskEntity>)null;
            availableTasks = (List<Netherite.Domain.Models.Task>)null;
            return taskList;
        }

        public async System.Threading.Tasks.Task<Guid> Complete(Guid userId, Guid taskId)
        {
            UserEntity userEntity = await this._context.Users.FindAsync(new object[1]
            {
        (object) userId
            });
            UserEntity user = userEntity ?? throw new Exception("Пользователь не найден");
            userEntity = (UserEntity)null;
            TaskEntity taskEntity = await this._context.Tasks.FindAsync(new object[1]
            {
        (object) taskId
            });
            TaskEntity task = taskEntity ?? throw new Exception("Задание не найдено");
            taskEntity = (TaskEntity)null;
            UserTaskEntity existingUserTaskEntity = await (System.Threading.Tasks.Task<UserTaskEntity>)EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<UserTaskEntity>(this._context.UserTasks, (ut => ut.UserId == userId && ut.TaskId == taskId), new CancellationToken());
            if (existingUserTaskEntity != null)
                throw new Exception("Задание данным пользователем уже выполнено");
            UserTaskEntity userTaskEntity = new UserTaskEntity()
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                User = user,
                TaskId = taskId,
                Task = task
            };
            EntityEntry<UserTaskEntity> entityEntry = await this._context.UserTasks.AddAsync(userTaskEntity, new CancellationToken());
            int reward = task.Reward;
            user.Balance += (Decimal)reward;
            UserEntity referrer = await (System.Threading.Tasks.Task<UserEntity>)EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<UserEntity>(this._context.Users, (u => (Guid?)u.Id == user.InvitedId), new CancellationToken());
            UserEntity referrersReferrer = (UserEntity)null;
            if (referrer != null)
                referrersReferrer = await (System.Threading.Tasks.Task<UserEntity>)EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<UserEntity>(this._context.Users, (u => (Guid?)u.Id == referrer.InvitedId), new CancellationToken());
            (int, int) valueTuple1 = await this._referalBonusesServices.Execute(user.IsPremium, reward);
            (int, int) valueTuple2 = valueTuple1;
            int referrerReward = valueTuple2.Item1;
            int referrersReferrerReward = valueTuple2.Item2;
            //valueTuple1 = ();
            //valueTuple2 = ();
            if (referrer != null)
            {
                referrer.Balance += (Decimal)referrerReward;
                user.Profit += referrerReward;
                if (referrersReferrer != null)
                {
                    referrersReferrer.Balance += (Decimal)referrersReferrerReward;
                    referrer.Profit += referrersReferrerReward;
                }
            }
            int num = await this._context.SaveChangesAsync(new CancellationToken());
            Guid id = userTaskEntity.Id;
            task = (TaskEntity)null;
            existingUserTaskEntity = (UserTaskEntity)null;
            userTaskEntity = (UserTaskEntity)null;
            referrersReferrer = (UserEntity)null;
            return id;
        }

        public async System.Threading.Tasks.Task<List<object>> GetReferals(Guid userId)
        {
            List<UserEntity> users = await (System.Threading.Tasks.Task<List<UserEntity>>)EntityFrameworkQueryableExtensions.ToListAsync<UserEntity>(((IQueryable<UserEntity>)EntityFrameworkQueryableExtensions.AsNoTracking<UserEntity>(this._context.Users)).Where<UserEntity>((Expression<Func<UserEntity, bool>>)(ut => ut.InvitedId == (Guid?)userId)), new CancellationToken());
            List<object> userDtos = new List<object>();
            foreach (UserEntity userEntity in users)
            {
                UserEntity entity = userEntity;
                int referalsCount = await EntityFrameworkQueryableExtensions.CountAsync<UserEntity>(((IQueryable<UserEntity>)EntityFrameworkQueryableExtensions.AsNoTracking<UserEntity>(this._context.Users)).Where<UserEntity>((Expression<Func<UserEntity, bool>>)(ut => ut.InvitedId == (Guid?)entity.Id)), new CancellationToken());
                var userDto = new
                {
                    id = entity.Id.ToString(),
                    profit = entity.Profit,
                    referals = referalsCount,
                    telegramName = entity.TelegramName
                };
                userDtos.Add((object)userDto);
                userDto = null;
            }
            List<object> objectList = userDtos;
            users = (List<UserEntity>)null;
            userDtos = (List<object>)null;
            return objectList;
        }

        public async System.Threading.Tasks.Task<User?> GetByWallet(string wallet)
        {
            UserEntity userEntity = await (System.Threading.Tasks.Task<UserEntity>)EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<UserEntity>(this._context.Users, u => u.Wallet == wallet);
            User user = userEntity != null ? this.MapToDomain(userEntity) : (User)null;
            userEntity = (UserEntity)null;
            return user;
        }

        private User MapToDomain(UserEntity userEntity)
        {
            (User User, string Error) = User.Create(userEntity.Id, userEntity.Balance, userEntity.Location, userEntity.InvitedId, userEntity.IsPremium, userEntity.TelegramId, userEntity.TelegramName, userEntity.Wallet, userEntity.Profit);
            if (!string.IsNullOrEmpty(Error))
                throw new Exception("User mapping error: " + Error);
            return User;
        }

        private UserEntity MapToEntity(User user) => new UserEntity()
        {
            Id = user.Id,
            Balance = user.Balance,
            Location = user.Location,
            InvitedId = user.InvitedId,
            IsPremium = user.IsPremium,
            TelegramId = user.TelegramId,
            TelegramName = user.TelegramName,
            Wallet = user.Wallet,
            Profit = user.Profit
        };
    }
}
