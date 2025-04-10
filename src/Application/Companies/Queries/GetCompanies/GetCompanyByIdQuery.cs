using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Companies._Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Company.Queries.GetCompanies
{
    public class GetCompanyByIdQuery : IRequest<CompanyDto>
    {
        public Guid Id { get; set; }

        public class GetCompanyByIdQueryHandler : IRequestHandler<GetCompanyByIdQuery, CompanyDto>
        {
            private readonly IApplicationDbContext _context;

            public GetCompanyByIdQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<CompanyDto> Handle(GetCompanyByIdQuery request, CancellationToken cancellationToken)
            {
                var company = await _context.Companies
                    .Where(c => c.Id == request.Id)
                    .Select(c => new CompanyDto
                    {
                        Id = c.Id,
                        CompanyName = c.CompanyName,
                        BusinessCategory = c.BusinessCategory,
                        CompanyCode = c.CompanyCode
                    })
                    .FirstOrDefaultAsync(cancellationToken);

                return company ?? throw new KeyNotFoundException("Company not found");
            }
        }
    }
}
