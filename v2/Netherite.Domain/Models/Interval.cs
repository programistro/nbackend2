using System;
using System.Runtime.CompilerServices;

namespace Netherite.Domain.Models
{
	public class Interval
	{
		private Interval(Guid id, int time, decimal interestRate)
		{
			this.Id = id;
			this.Time = time;
			this.InterestRate = interestRate;
		}

		public Guid Id { get; set; }

		public int Time { get; set; }

		public decimal InterestRate { get; set; }

		public static Interval Create(Guid id, int time, decimal interestRate)
		{
			string error = string.Empty;
			return new Interval(id, time, interestRate);
		}
	}
}
