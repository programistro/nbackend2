using System;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Netherite.Infrastructure.Entities;

namespace Netherite.Infrastructure.Configurations
{
	public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
	{	
		public void Configure(EntityTypeBuilder<UserEntity> builder)
		{
			builder.HasKey((UserEntity x) => (object)x.Id);
			builder.Property<decimal>((UserEntity b) => b.Balance).HasDefaultValue(0).IsRequired(true);
			builder.Property<string>((UserEntity b) => b.Location).IsRequired(true);
			builder.Property<Guid?>((UserEntity b) => b.InvitedId).IsRequired(false);
			builder.Property<bool>((UserEntity b) => b.IsPremium).HasDefaultValue(false).IsRequired(true);
			builder.Property<string>((UserEntity b) => b.TelegramId).IsRequired(true);
			builder.Property<string>((UserEntity b) => b.TelegramName).IsRequired(true);
			builder.Property<string>((UserEntity b) => b.Wallet).IsRequired(true);
			builder.HasMany<UserTaskEntity>((UserEntity u) => u.UserTasks).WithOne((UserTaskEntity ut) => ut.User).HasForeignKey((UserTaskEntity ut) => (object)ut.UserId);
			builder.HasMany<FavoritesEntity>((UserEntity u) => u.Favorites).WithOne((FavoritesEntity ut) => ut.User).HasForeignKey((FavoritesEntity ut) => (object)ut.UserId);
		}
	}
}
