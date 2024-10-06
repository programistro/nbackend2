using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Netherite.API.Contracts
{
	public class UserRequest 
	{
		public UserRequest(string Location, Guid? InvitedId, bool IsPremium, string TelegramId, string TelegramName, string Wallet)
		{
			this.Location = Location;
			this.InvitedId = InvitedId;
			this.IsPremium = IsPremium;
			this.TelegramId = TelegramId;
			this.TelegramName = TelegramName;
			this.Wallet = Wallet;
		}		

		public string Location { get; set; }

		public Guid? InvitedId { get; set; }

		public bool IsPremium { get; set; }

		public string TelegramId { get; set; }

		public string TelegramName { get; set; }

		public string Wallet { get; set; }			
	}
}
