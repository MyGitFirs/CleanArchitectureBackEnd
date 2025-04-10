using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Application.Transactions._Dto
{
    public class TransactionDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid KioskId { get; set; }
        public string KioskName { get; set; }
        public string Category { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
