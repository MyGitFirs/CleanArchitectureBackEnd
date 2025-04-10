using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Domain.Entities
{
    public class Wallet : BaseEntity
    {
        public Guid UserID { get; set; }
        public decimal Balance { get; set; }
        public decimal MonthlyLimit { get; set; }
        public DateTime LastResetDate { get; set; }
    }
}
