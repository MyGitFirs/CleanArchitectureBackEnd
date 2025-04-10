using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Companies._Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Company.Queries.GetCompanies
{
    public class GetCompanyQuery : IRequest<List<CompanyDto>>
    {
        public string SortBy { get; set; }
        public List<Guid> CompanyIds { get; set; }

        public class GetCompanyQueryHandler : IRequestHandler<GetCompanyQuery, List<CompanyDto>>
        {
            private readonly IApplicationDbContext _context;

            public GetCompanyQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<List<CompanyDto>> Handle(GetCompanyQuery request, CancellationToken cancellationToken)
            {
                var response = new List<CompanyDto>();
                var qry = _context.Companies.AsQueryable();

                if (request.CompanyIds != null && request.CompanyIds.Any())
                {
                    qry = qry.Where(cp => request.CompanyIds.Contains(cp.Id));
                }

                var output = await qry.Select(cp => new CompanyDto
                {
                    Id = cp.Id,
                    CompanyName = cp.CompanyName,
                    BusinessCategory = cp.BusinessCategory,
                    CompanyCode = cp.CompanyCode
                }).ToListAsync(cancellationToken);

                response = output;
                return response;
            }
        }
    }
}
