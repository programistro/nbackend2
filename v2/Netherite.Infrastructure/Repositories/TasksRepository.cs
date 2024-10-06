
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Netherite.Domain.Repositories;
using Netherite.Infrastructure.Entities;

#nullable enable
namespace Netherite.Infrastructure.Repositories
{
    public class TasksRepository : ITasksRepository
    {
        private readonly NetheriteDbContext _context;

        public TasksRepository(NetheriteDbContext context) => this._context = context;

        public async System.Threading.Tasks.Task<List<Netherite.Domain.Models.Task>> Get()
        {
            List<TaskEntity> taskEntities = await (System.Threading.Tasks.Task<List<TaskEntity>>)EntityFrameworkQueryableExtensions.ToListAsync<TaskEntity>(EntityFrameworkQueryableExtensions.AsNoTracking<TaskEntity>(this._context.Tasks), new CancellationToken());
            List<Netherite.Domain.Models.Task> tasks = taskEntities.Select<TaskEntity, Netherite.Domain.Models.Task>((Func<TaskEntity, Netherite.Domain.Models.Task>)(t => Netherite.Domain.Models.Task.Create(t.Id, t.Title, t.Description, t.Icon, t.Reward, t.Status, t.Link).Task)).ToList<Netherite.Domain.Models.Task>();
            List<Netherite.Domain.Models.Task> taskList = tasks;
            taskEntities = (List<TaskEntity>)null;
            tasks = (List<Netherite.Domain.Models.Task>)null;
            return taskList;
        }

        public async System.Threading.Tasks.Task<Guid> Create(Netherite.Domain.Models.Task task)
        {
            TaskEntity taskEntity = new TaskEntity()
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Icon = task.Icon,
                Reward = task.Reward,
                Status = task.Status,
                Link = task.Link
            };
            EntityEntry<TaskEntity> entityEntry = await this._context.Tasks.AddAsync(taskEntity, new CancellationToken());
            int num = await this._context.SaveChangesAsync(new CancellationToken());
            Guid id = taskEntity.Id;
            taskEntity = (TaskEntity)null;
            return id;
        }

        public async System.Threading.Tasks.Task<bool> Delete(Guid taskId)
        {
            TaskEntity taskEntity = await this._context.Tasks.FindAsync(new object[1]
            {
        (object) taskId
            });
            if (taskEntity == null)
                return false;
            this._context.Tasks.Remove(taskEntity);
            int num = await this._context.SaveChangesAsync(new CancellationToken());
            return true;
        }

        public async System.Threading.Tasks.Task<bool> Update(Guid taskId, Netherite.Domain.Models.Task task)
        {
            TaskEntity taskEntity = await this._context.Tasks.FindAsync(new object[1]
            {
        (object) taskId
            });
            if (taskEntity == null)
                return false;
            taskEntity.Title = task.Title;
            taskEntity.Description = task.Description;
            taskEntity.Icon = task.Icon;
            taskEntity.Reward = task.Reward;
            taskEntity.Status = task.Status;
            taskEntity.Link = task.Link;

            this._context.Tasks.Update(taskEntity);
            int num = await this._context.SaveChangesAsync(new CancellationToken());
            return true;
        }
    }
}
