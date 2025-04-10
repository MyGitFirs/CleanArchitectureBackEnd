using System;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Kiosks.Commands.UpdateKiosk
{
    public class UpdateKioskCommand : IRequest
    {
        public Guid Id { get; set; }
        public string KioskName { get; set; }
        public string Category { get; set; }
        public Guid CompanyId { get; set; }
        public bool Status { get; set; }

        public class UpdateKioskCommandHandler : IRequestHandler<UpdateKioskCommand>
        {
            private readonly IApplicationDbContext _context;

            public UpdateKioskCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(UpdateKioskCommand request, CancellationToken cancellationToken)
            {
                var kiosk = await _context.Kiosks.FirstOrDefaultAsync(k => k.Id == request.Id, cancellationToken);

                if (kiosk == null)
                    throw new Exception("Kiosk not found");

                kiosk.KioskName = request.KioskName;
                kiosk.Category = request.Category;
                kiosk.CompanyId = request.CompanyId;
                kiosk.Status = request.Status;
                kiosk.UpdatedDate = DateTime.Now;

                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}