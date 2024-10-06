using Netherite.Domain.Models;

namespace Netherite.API.Contracts
{
	public class TasksRequest 
	{
		public TasksRequest(string Title, string Description, string Icon, int Reward, string Status, string Link)
		{
			this.Title = Title;
			this.Description = Description;
			this.Icon = Icon;
			this.Reward = Reward;
			this.Status = Status;
			this.Link = Link;
		}

        public TasksRequest()
		{
		}

        public string Title { get; set; }

		public string Description { get; set; }

		public string Icon { get; set; }

		public int Reward { get; set; }

		public string Status { get; set; }

		public string Link { get; set; }
	}
}
