using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Domain.Entities
{
    public class Transaction : BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid KioskId { get; set; }
        public Kiosk Kiosk { get; set; }
        public decimal Amount { get; set; }


    }
}
