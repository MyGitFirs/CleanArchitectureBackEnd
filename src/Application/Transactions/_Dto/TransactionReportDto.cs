using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Application.Transactions._Dto
{
    public class TransactionReportDto
    {
        public string KioskName { get; set; }
        public string Category { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal Amount { get; set; }
        
    }
}
