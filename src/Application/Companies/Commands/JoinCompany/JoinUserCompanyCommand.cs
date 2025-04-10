using System;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Companies.Commands.JoinCompany
{
    public class JoinUserCompanyCommand : IRequest<string>
    {
        public Guid UserId { get; set; }
        public Guid CompanyId { get; set; }

        public class JoinUserCompanyCmdHandler : IRequestHandler<JoinUserCompanyCommand, string>
        {
            private readonly IApplicationDbContext _context;

            public JoinUserCompanyCmdHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<string> Handle(JoinUserCompanyCommand request, CancellationToken cancellationToken)
            {
                
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);
                if (user == null)
                {
                    throw new Exception("User not found.");
                }

                
                var company = await _context.Companies.FirstOrDefaultAsync(c => c.Id == request.CompanyId, cancellationToken);
                if (company == null)
                {
                    throw new Exception("Company not found.");
                }

                
                user.CompanyID = request.CompanyId;
                user.UpdatedDate = DateTime.Now;

                _context.Users.Update(user);
                await _context.SaveChangesAsync(cancellationToken);

                return $"User {user.FirstName} {user.LastName} has joined Company {company.CompanyName}.";
            }
        }
    }
}
