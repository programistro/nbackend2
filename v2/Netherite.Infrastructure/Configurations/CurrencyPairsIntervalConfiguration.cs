using System;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Netherite.Infrastructure.Entities;

namespace Netherite.Infrastructure.Configurations
{
	public class CurrencyPairsIntervalConfiguration : IEntityTypeConfiguration<CurrencyPairsIntervalEntity>
	{		
		public void Configure(EntityTypeBuilder<CurrencyPairsIntervalEntity> builder)
		{
			builder.ToTable("CurrencyPairsIntervals");
			builder.HasKey((CurrencyPairsIntervalEntity ut) => (object)ut.Id);
			builder.HasOne<CurrencyPairsEntity>((CurrencyPairsIntervalEntity cpi) => cpi.CurrencyPairs).WithMany((CurrencyPairsEntity cp) => cp.CurrencyPairsIntervals).HasForeignKey((CurrencyPairsIntervalEntity cpi) => (object)cpi.CurrencyPairsId);
			builder.HasOne<IntervalEntity>((CurrencyPairsIntervalEntity cpi) => cpi.Interval).WithMany((IntervalEntity i) => i.CurrencyPairsIntervals).HasForeignKey((CurrencyPairsIntervalEntity cpi) => (object)cpi.IntervalId);
		}		
	}
}
