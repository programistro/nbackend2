using System;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Netherite.Infrastructure.Entities;

namespace Netherite.Infrastructure.Configurations
{	
	public class OrderConfiguration : IEntityTypeConfiguration<OrderEntity>
	{		
		public void Configure(EntityTypeBuilder<OrderEntity> builder)
		{
			builder.ToTable("Orders");
			builder.HasKey((OrderEntity x) => (object)x.Id);
		}		
	}
}
