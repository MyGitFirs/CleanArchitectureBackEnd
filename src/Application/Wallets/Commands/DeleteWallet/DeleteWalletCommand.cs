using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Wallets.Commands.DeleteWallet
{
    public class DeleteWalletCommand : IRequest
    {
        public Guid Id { get; set; }
    }

    public class DeleteWalletCommandHandler : IRequestHandler<DeleteWalletCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteWalletCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteWalletCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Wallets
                .Where(w => w.Id == request.Id)
                .SingleOrDefaultAsync(cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Wallet), request.Id);
            }

            _context.Wallets.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
