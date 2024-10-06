using System;
using System.Runtime.CompilerServices;

namespace Netherite.Infrastructure.Entities
{
	public class UserTaskEntity
	{
		public Guid Id { get; set; }

		public Guid UserId { get; set; }

		public UserEntity User { get; set; } = null;

		public Guid TaskId { get; set; }

		public TaskEntity Task { get; set; } = null;
		
	}
}
