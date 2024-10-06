using System;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Netherite.Infrastructure.Entities;

namespace Netherite.Infrastructure.Configurations
{
	public class UserTaskConfiguration
	{
		public void Configure(EntityTypeBuilder<UserTaskEntity> builder)
		{
			builder.HasKey((UserTaskEntity ut) => (object)ut.Id);
			builder.HasOne<UserEntity>((UserTaskEntity ut) => ut.User).WithMany((UserEntity u) => u.UserTasks).HasForeignKey((UserTaskEntity ut) => (object)ut.UserId);
			builder.HasOne<TaskEntity>((UserTaskEntity ut) => ut.Task).WithMany((TaskEntity t) => t.UserTasks).HasForeignKey((UserTaskEntity ut) => (object)ut.TaskId);
		}		
	}
}
