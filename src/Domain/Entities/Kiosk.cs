using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Domain.Entities
{
    public class Kiosk : BaseEntity
    {
        public Guid CompanyId { get; set; }
        public string KioskName { get; set; }
        public string Category { get; set; }
        public bool Status { get; set; }

        
    }
}
