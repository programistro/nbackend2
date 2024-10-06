
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;


#nullable enable
namespace Netherite.API.Contracts
{
    public class UserResponse 
    {
        public UserResponse(
          Guid Id,
          Decimal Balance,

#nullable enable
          string Location,
          Guid? InvitedId,
          bool IsPremium,
          string TelegramId,
          string TelegramName,
          string Wallet,
          int Profit)
        {
            // ISSUE: reference to a compiler-generated field
            this.Id = Id;
            // ISSUE: reference to a compiler-generated field
            this.Balance = Balance;
            // ISSUE: reference to a compiler-generated field
            this.Location = Location;
            // ISSUE: reference to a compiler-generated field
            this.InvitedId = InvitedId;
            // ISSUE: reference to a compiler-generated field
            this.IsPremium = IsPremium;
            // ISSUE: reference to a compiler-generated field
            this.TelegramId = TelegramId;
            // ISSUE: reference to a compiler-generated field
            this.TelegramName = TelegramName;
            // ISSUE: reference to a compiler-generated field
            this.Wallet = Wallet;
            // ISSUE: reference to a compiler-generated field
            this.Profit = Profit;
            // ISSUE: explicit constructor call            
        }       

        public Guid Id { get; init; }

        public Decimal Balance { get; init; }

        public string Location { get; init; }

        public Guid? InvitedId { get; init; }

        public bool IsPremium { get; init; }

        public string TelegramId { get; init; }

        public string TelegramName { get; init; }

        public string Wallet { get; init; }

        public int Profit { get; init; }             
    }
}
