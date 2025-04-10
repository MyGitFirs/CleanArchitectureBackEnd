using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Kiosk._Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Kiosks.Queries.GetKiosks
{
    public class GetKioskByIdQuery : IRequest<KioskDto>
    {
        public Guid Id { get; set; }

        public class GetKioskByIdQueryHandler : IRequestHandler<GetKioskByIdQuery, KioskDto>
        {
            private readonly IApplicationDbContext _context;

            public GetKioskByIdQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<KioskDto> Handle(GetKioskByIdQuery request, CancellationToken cancellationToken)
            {
                var kiosk = await _context.Kiosks
                    .Where(k => k.Id == request.Id)
                    .Select(k => new KioskDto
                    {
                        Id = k.Id,
                        KioskName = k.KioskName,
                        Category = k.Category,
                        Status = k.Status
                    })
                    .FirstOrDefaultAsync(cancellationToken);

                return kiosk ?? throw new KeyNotFoundException("Kiosk not found");
            }
        }
    }
}