using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Company.Commands.UpdateCompany
{
    public class UpdateCompanyCommand : IRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string BusinessCategory { get; set; }
        

        public class UpdateCompanyCommandHandler : IRequestHandler<UpdateCompanyCommand>
        {
            private readonly IApplicationDbContext _context;

            public UpdateCompanyCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
            {
                var company = await _context.Companies.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

                if (company == null)
                    throw new Exception("Company not found");

                company.CompanyName = request.Name;
                company.BusinessCategory = request.BusinessCategory;
                company.UpdatedDate = DateTime.Now;
                

                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }

        }

    }
}