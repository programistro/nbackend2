using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Netherite.Infrastructure.Entities;

namespace Netherite.Infrastructure.Configurations
{	
	public class CurrencyPairsConfiguration : IEntityTypeConfiguration<CurrencyPairsEntity>
	{		
		public void Configure(EntityTypeBuilder<CurrencyPairsEntity> builder)
		{            
			builder.ToTable("CurrencyPairs");
			builder.HasKey((CurrencyPairsEntity x) => (object)x.Id);
			builder.HasMany<CurrencyPairsIntervalEntity>((CurrencyPairsEntity cpi) => cpi.CurrencyPairsIntervals).WithOne((CurrencyPairsIntervalEntity cp) => cp.CurrencyPairs).HasForeignKey((CurrencyPairsIntervalEntity cp) => (object)cp.CurrencyPairsId);
			builder.HasMany<FavoritesEntity>((CurrencyPairsEntity cpi) => cpi.Favorites).WithOne((FavoritesEntity cp) => cp.CurrencyPairs).HasForeignKey((FavoritesEntity cp) => (object)cp.CurrencyPairsId);            
        }
    }
}
