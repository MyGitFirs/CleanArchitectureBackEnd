using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Application.Kiosk._Dto
{
    public class KioskDto
    {
        public Guid Id { get; set; }
        public string KioskName { get; set; }
        public string Category { get; set; }
        public bool Status { get; set; }
        
    }
}
