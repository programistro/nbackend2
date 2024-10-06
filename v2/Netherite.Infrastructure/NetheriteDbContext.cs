using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Netherite.Infrastructure.Entities;

namespace Netherite.Infrastructure
{		
	public class NetheriteDbContext : DbContext
	{		
		public NetheriteDbContext(DbContextOptions<NetheriteDbContext> options) : base(options)
		{
		}
		
		public DbSet<OrderEntity> Orders { get; set; }
		
		public DbSet<TaskEntity> Tasks { get; set; }
		
		public DbSet<MinerEntity> Miners { get; set; }
		
		public DbSet<UserEntity> Users { get; set; }
		
		public DbSet<UserTaskEntity> UserTasks { get; set; }
		
		public DbSet<IntervalEntity> Intervals { get; set; }
		
		public DbSet<CurrencyPairsEntity> CurrencyPairs { get; set; }
		
		public DbSet<CurrencyPairsIntervalEntity> CurrencyPairsIntervals { get; set; }
		
		public DbSet<FavoritesEntity> Favorites { get; set; }
	}
}
