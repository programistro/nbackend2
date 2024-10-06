using System;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Netherite.Infrastructure.Entities;

namespace Netherite.Infrastructure.Configurations
{
	public class TasksConfiguration : IEntityTypeConfiguration<TaskEntity>
	{		
		public void Configure(EntityTypeBuilder<TaskEntity> builder)
		{
			builder.HasKey((TaskEntity x) => (object)x.Id);
			builder.Property<string>((TaskEntity b) => b.Title).IsRequired(true);
			builder.Property<string>((TaskEntity b) => b.Description).IsRequired(true);
			builder.Property<int>((TaskEntity b) => b.Reward).IsRequired(true);
			builder.HasMany<UserTaskEntity>((TaskEntity t) => t.UserTasks).WithOne((UserTaskEntity ut) => ut.Task).HasForeignKey((UserTaskEntity ut) => (object)ut.TaskId);
		}
	}
}
