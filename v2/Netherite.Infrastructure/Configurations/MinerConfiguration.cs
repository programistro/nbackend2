using System;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Netherite.Infrastructure.Entities;

namespace Netherite.Infrastructure.Configurations
{
	public class MinerConfiguration : IEntityTypeConfiguration<MinerEntity>
	{				
		public void Configure(EntityTypeBuilder<MinerEntity> builder)
		{
			builder.HasKey((MinerEntity x) => (object)x.Id);
			builder.Property<int>((MinerEntity b) => b.Reward).IsRequired(true);
			builder.Property<DateTime>((MinerEntity b) => b.StartTime).IsRequired(true);
			builder.Property<DateTime>((MinerEntity b) => b.EndTime).IsRequired(true);
		}
	}
}
