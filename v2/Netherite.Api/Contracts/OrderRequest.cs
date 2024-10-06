using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Netherite.API.Contracts
{
	public class OrderRequest 
	{
		public OrderRequest(Guid UserId, Guid CurrencyPairsId, Guid IntervalId, int Bet, bool PurchaseDirection)
		{
			this.UserId = UserId;
			this.CurrencyPairsId = CurrencyPairsId;
			this.Bet = Bet;
			this.PurchaseDirection = PurchaseDirection;
			this.IntervalId = IntervalId;
		}

		public OrderRequest()
		{

		}

		public Guid UserId { get; set; }

		public Guid CurrencyPairsId { get; set; }

		public Guid IntervalId { get; set; }		

		public int Bet { get; set; }

		public bool PurchaseDirection { get; set; }		
	}
}
