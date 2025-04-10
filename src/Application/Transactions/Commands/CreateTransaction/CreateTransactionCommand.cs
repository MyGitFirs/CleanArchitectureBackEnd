using System;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Transactions.Commands
{
    public class CreateTransactionCommand : IRequest<string>
    {
        public Guid UserId { get; set; }
        public Guid KioskId { get; set; }
        public decimal Amount { get; set; }

        public class CreateTransactionCmdHandler : IRequestHandler<CreateTransactionCommand, string>
        {
            private readonly IApplicationDbContext _context;
            private readonly INotificationService _notificationService;

            public CreateTransactionCmdHandler(IApplicationDbContext context, INotificationService notificationService)
            {
                _context = context;
                _notificationService = notificationService;
            }

            public async Task<string> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
            {
                // Fetch the user's wallet
                var userWallet = await _context.Wallets
                    .FirstOrDefaultAsync(w => w.UserID == request.UserId, cancellationToken);

                if (userWallet == null)
                {
                    throw new Exception("Wallet not found.");
                }

                // Check if the user has enough balance
                if (userWallet.Balance < request.Amount)
                {
                    throw new Exception("Insufficient balance.");
                }

                // Deduct the amount from wallet
                userWallet.Balance -= request.Amount;

                // Create a new transaction
                var transaction = new CleanArchitecture.Domain.Entities.Transaction
                {
                    Id = Guid.NewGuid(),
                    UserId = request.UserId,
                    KioskId = request.KioskId,
                    Amount = request.Amount,
                    CreatedDate = DateTime.UtcNow
                };

                _context.Transactions.Add(transaction);
                _context.Wallets.Update(userWallet); // Update the wallet balance

                await _context.SaveChangesAsync(cancellationToken);

                // Send real-time notification to the user via OneSignal
                try
                {
                    await _notificationService.SendNotificationAsync(
                        request.UserId,
                        $"₱{request.Amount:N} spent at Canteen. Balance: ₱{userWallet.Balance:N}",
                        cancellationToken
                    );
                }
                catch (Exception ex)
                {
                    
                    Console.WriteLine($"Failed to send notification: {ex.Message}");
                }

                return transaction.Id.ToString();
            }
        }
    }
}
