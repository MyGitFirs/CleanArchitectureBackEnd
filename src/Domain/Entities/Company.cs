using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Domain.Entities
{
    public class Company : BaseEntity
    {
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string BusinessCategory { get; set; }
    }
}
