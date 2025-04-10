using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Application.Companies._Dto
{
    public class CompanyDto
    {
        public Guid Id { get; set; }
        public string CompanyName { get; set; }
        public string BusinessCategory { get; set; }
        public string CompanyCode { get; set; }

    }
}
