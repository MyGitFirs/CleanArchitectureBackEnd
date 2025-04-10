using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Kiosks.Commands.DeleteKiosk
{
    public class DeleteKioskCommand : IRequest
    {
        public Guid Id { get; set; }
    }

    public class DeleteKioskCommandHandler : IRequestHandler<DeleteKioskCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteKioskCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteKioskCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Kiosks
                .Where(k => k.Id == request.Id)
                .SingleOrDefaultAsync(cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Kiosk), request.Id);
            }

            _context.Kiosks.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}