namespace Netherite.API.Contracts
{
	public class IntervalsRequest 
	{
		public IntervalsRequest(int Time, decimal InterestRate)
		{
			this.Time = Time;
			this.InterestRate = InterestRate;
		}

		public IntervalsRequest()
		{

		}

		public int Time { get; set; }

		public decimal InterestRate { get; set; }
	}
}
