using System;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Netherite.Infrastructure.Entities;

namespace Netherite.Infrastructure.Configurations
{	
	public class FavoritesConfiguration : IEntityTypeConfiguration<FavoritesEntity>
	{		
		public void Configure(EntityTypeBuilder<FavoritesEntity> builder)
		{
			builder.ToTable("Favorites");
			builder.HasKey((FavoritesEntity ut) => (object)ut.Id);
			builder.HasOne<UserEntity>((FavoritesEntity f) => f.User).WithMany((UserEntity u) => u.Favorites).HasForeignKey((FavoritesEntity f) => (object)f.UserId);
			builder.HasOne<CurrencyPairsEntity>((FavoritesEntity f) => f.CurrencyPairs).WithMany((CurrencyPairsEntity cp) => cp.Favorites).HasForeignKey((FavoritesEntity f) => (object)f.CurrencyPairsId);
		}
	}
}
