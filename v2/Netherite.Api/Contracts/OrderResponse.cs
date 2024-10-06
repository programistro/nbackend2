namespace Netherite.API.Contracts
{
	public class OrderResponse
	{
		public OrderResponse(Guid Id, Guid UserId, Guid CurrencyPairsId, Guid IntervalId, int Bet, decimal StartPrice, DateTime StartTime, DateTime EndTime, bool PurchaseDirection, bool Ended)
		{
			this.Id = Id;
			this.UserId = UserId;
			this.CurrencyPairsId = CurrencyPairsId;
			this.IntervalId = IntervalId;
			this.Bet = Bet;
			this.StartPrice = StartPrice;
			this.StartTime = StartTime;
			this.EndTime = EndTime;
			this.PurchaseDirection = PurchaseDirection;
			this.Ended = Ended;
		}		

		public Guid Id { get; set; }

		public Guid UserId { get; set; }

		public Guid CurrencyPairsId { get; set; }

		public Guid IntervalId { get; set; }

		public int Bet { get; set; }

		public decimal StartPrice { get; set; }

		public DateTime StartTime { get; set; }

		public DateTime EndTime { get; set; }

		public bool PurchaseDirection { get; set; }

		public bool Ended { get; set; }
	}
}
