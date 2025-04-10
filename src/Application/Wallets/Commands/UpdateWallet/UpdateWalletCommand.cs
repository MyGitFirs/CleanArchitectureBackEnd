using System;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Wallets.Commands.UpdateWallet
{
    public class UpdateWalletCommand : IRequest
    {
        public Guid Id { get; set; }
        public decimal Balance { get; set; }
        public decimal MonthlyLimit { get; set; }

        public class UpdateWalletCommandHandler : IRequestHandler<UpdateWalletCommand>
        {
            private readonly IApplicationDbContext _context;

            public UpdateWalletCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(UpdateWalletCommand request, CancellationToken cancellationToken)
            {
                var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.Id == request.Id, cancellationToken);

                if (wallet == null)
                    throw new Exception("Wallet not found");

                wallet.Balance = request.Balance;
                wallet.MonthlyLimit = request.MonthlyLimit;
                wallet.UpdatedDate = DateTime.Now;

                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
