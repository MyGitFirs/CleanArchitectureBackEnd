using System;
using System.Collections.Generic;
using System.Text;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Companies.Commands.CreateCompany
{
    public class CreateCompanyCommand : IRequest<string>
    {
        public string CompanyName { get; set; }
        public string BusinessCategory { get; set; }
    }

    public class CreateCompanyCommandHandler : IRequestHandler<CreateCompanyCommand, string>
    {
        private readonly IApplicationDbContext _context;

        public CreateCompanyCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
        {
            var entity = new CleanArchitecture.Domain.Entities.Company
            {
                CompanyName = request.CompanyName,
                BusinessCategory = request.BusinessCategory,
                CompanyCode = await GenerateUniqueCompanyCode(),
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now
            };

            _context.Companies.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id.ToString();
        }
        public async Task<string> GenerateUniqueCompanyCode()
        {
            string code;
            bool exists;

            do
            {
                code = Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();
                exists = await _context.Companies.AnyAsync(c => c.CompanyCode == code);
            } while (exists);

            return code;
        }

    }
}
