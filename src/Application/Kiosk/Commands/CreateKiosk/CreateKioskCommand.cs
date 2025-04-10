using System;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Interfaces;
using MediatR;

namespace CleanArchitecture.Application.Kiosks.Commands
{
    public class CreateKioskCommand : IRequest<string>
    {
        public string KioskName { get; set; }
        public string Category { get; set; }
        public Guid CompanyId { get; set; }
        public bool Status { get; set; }

        public class CreateKioskCmdHandler : IRequestHandler<CreateKioskCommand, string>
        {
            private readonly IApplicationDbContext _context;

            public CreateKioskCmdHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<string> Handle(CreateKioskCommand request, CancellationToken cancellationToken)
            {
                var kiosk = new CleanArchitecture.Domain.Entities.Kiosk
                {
                    Id = Guid.NewGuid(),
                    KioskName = request.KioskName,
                    Category = request.Category,
                    CompanyId = request.CompanyId,
                    Status = request.Status,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now
                };

                _context.Kiosks.Add(kiosk);
                await _context.SaveChangesAsync(cancellationToken);

                return kiosk.Id.ToString();
            }
        }
    }
}
