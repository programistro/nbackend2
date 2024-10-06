using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Netherite.Infrastructure.Entities
{
	public class UserEntity
	{
		public Guid Id { get; set; }

		public decimal Balance { get; set; }

		public string Location { get; set; } = null;

		public Guid? InvitedId { get; set; } = new Guid?(Guid.Empty);

		public bool IsPremium { get; set; }

		public string TelegramId { get; set; } = null;

		public string TelegramName { get; set; } = null;

		public string Wallet { get; set; } = null;

		public int Profit { get; set; }

		public ICollection<UserTaskEntity> UserTasks { get; set; } = new List<UserTaskEntity>();

		public ICollection<FavoritesEntity> Favorites { get; set; } = new List<FavoritesEntity>();

		public UserEntity()
		{
		}
	}
}
