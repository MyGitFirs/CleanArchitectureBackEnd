using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Wallets.Commands
{
    public class CreateWalletForCompanyCommand : IRequest<int> 
    {
        public Guid CompanyId { get; set; }
        public decimal InitialBalance { get; set; }
        public decimal MonthlyLimit { get; set; }

        public class CreateWalletForCompanyCmdHandler : IRequestHandler<CreateWalletForCompanyCommand, int>
        {
            private readonly IApplicationDbContext _context;

            public CreateWalletForCompanyCmdHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<int> Handle(CreateWalletForCompanyCommand request, CancellationToken cancellationToken)
            {
                
                var usersWithoutWallets = await _context.Users
                    .Where(u => u.CompanyID == request.CompanyId)
                    .Where(u => !_context.Wallets.Any(w => w.UserID == u.Id)) 
                    .ToListAsync(cancellationToken);

                if (!usersWithoutWallets.Any())
                    return 0; 

                foreach (var user in usersWithoutWallets)
                {
                    var wallet = new CleanArchitecture.Domain.Entities.Wallet
                    {
                        Id = Guid.NewGuid(),
                        UserID = user.Id,
                        Balance = request.InitialBalance,
                        MonthlyLimit = request.MonthlyLimit,
                        LastResetDate = DateTime.Now,
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now
                    };

                    _context.Wallets.Add(wallet);
                }

                await _context.SaveChangesAsync(cancellationToken);

                return usersWithoutWallets.Count; 
            }
        }
    }
}
