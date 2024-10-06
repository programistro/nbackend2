using System;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Netherite.Infrastructure.Entities;

namespace Netherite.Infrastructure.Configurations
{	
	public class IntervalConfiguration : IEntityTypeConfiguration<IntervalEntity>
	{
		public void Configure(EntityTypeBuilder<IntervalEntity> builder)
		{
			builder.ToTable("Intervals");
			builder.HasKey((IntervalEntity x) => (object)x.Id);
			builder.HasMany<CurrencyPairsIntervalEntity>((IntervalEntity cpi) => cpi.CurrencyPairsIntervals).WithOne((CurrencyPairsIntervalEntity i) => i.Interval).HasForeignKey((CurrencyPairsIntervalEntity i) => (object)i.IntervalId);
		}	
	}
}
