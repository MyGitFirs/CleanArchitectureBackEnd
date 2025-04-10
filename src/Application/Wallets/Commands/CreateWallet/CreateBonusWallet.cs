using System;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Wallets.Commands
{
    public class CreateBonusWallet : IRequest<string>
    {
        public Guid UserId { get; set; }
        public decimal BonusAmount { get; set; }

        public class AddBonusBalanceCmdHandler : IRequestHandler<CreateBonusWallet, string>
        {
            private readonly IApplicationDbContext _context;

            public AddBonusBalanceCmdHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<string> Handle(CreateBonusWallet request, CancellationToken cancellationToken)
            {
                var wallet = await _context.Wallets
                    .FirstOrDefaultAsync(w => w.UserID == request.UserId, cancellationToken);

                if (wallet == null)
                {
                    throw new Exception("Wallet not found.");
                }

                wallet.Balance += request.BonusAmount;
                wallet.UpdatedDate = DateTime.Now;

                _context.Wallets.Update(wallet);
                await _context.SaveChangesAsync(cancellationToken);

                return $"Bonus of {request.BonusAmount:C} added. New Balance: {wallet.Balance:C}";
            }
        }
    }
}
