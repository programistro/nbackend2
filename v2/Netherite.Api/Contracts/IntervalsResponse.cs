namespace Netherite.API.Contracts
{
	public class IntervalsResponse
	{
		public IntervalsResponse(Guid Id, int Time, decimal InterestRate)
		{
			this.Id = Id;
			this.Time = Time;
			this.InterestRate = InterestRate;
		}		

		public Guid Id { get; set; }

		public int Time { get; set; }

		public decimal InterestRate { get; set; }
	}
}
