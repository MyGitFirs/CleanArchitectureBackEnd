using System;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Interfaces;
using MediatR;

namespace CleanArchitecture.Application.Wallets.Commands
{
    public class CreateWalletCommand : IRequest<string>
    {
        public Guid UserId { get; set; }
        public decimal Balance { get; set; }
        public decimal MonthlyLimit { get; set; }

        public class CreateWalletCmdHandler : IRequestHandler<CreateWalletCommand, string>
        {
            private readonly IApplicationDbContext _context;

            public CreateWalletCmdHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<string> Handle(CreateWalletCommand request, CancellationToken cancellationToken)
            {
                var wallet = new CleanArchitecture.Domain.Entities.Wallet
                {
                    Id = Guid.NewGuid(),
                    UserID = request.UserId,
                    Balance = request.Balance,
                    MonthlyLimit = request.MonthlyLimit,
                    LastResetDate = DateTime.Now,
                    CreatedDate = DateTime.Now,
                };

                _context.Wallets.Add(wallet);
                await _context.SaveChangesAsync(cancellationToken);

                return wallet.Id.ToString();
            }
        }
    }
}
