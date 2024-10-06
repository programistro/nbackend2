using Netherite.Domain.Models;

namespace Netherite.Infrastructure.Entities
{
	public class TaskEntity
	{
		public Guid Id { get; set; }

		public string Title { get; set; } = null;

		public string Description { get; set; } = null;

		public string Icon { get; set; } = null;

		public int Reward { get; set; } = 0;

		public string Status { get; set; }

		public string Link { get; set; }

		public ICollection<UserTaskEntity> UserTasks { get; set; } = new List<UserTaskEntity>();

	}
}
