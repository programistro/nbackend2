using Netherite.Domain.Models;

namespace Netherite.API.Contracts
{
	public class TasksResponse
	{
		public TasksResponse(Guid Id, string Title, string Description, string Icon, int Reward, string Status, string Link) { 
			this.Id = Id;
			this.Title = Title;
			this.Description = Description;
			this.Icon = Icon;
			this.Reward = Reward;
			this.Status = Status;
			this.Link = Link;
		}		

		public Guid Id { get; set; }

		public string Title { get; set; }

		public string Description { get; set; }

		public string Icon { get; set; }

		public int Reward { get; set; }

		public string Status { get; set; }

		public string Link { get; set; }
	}
}
