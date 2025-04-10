using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Application.Wallets._Dto
{
    public class WalletDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public decimal Balance { get; set; }
        public decimal MonthlyLimit { get; set; }
        public DateTime LastResetDate { get; set; }
    }
}
