using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Kiosk._Dto;
using CleanArchitecture.Application.User._Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Kiosks.Queries.GetKiosks
{
    public class GetKioskQuery : IRequest<List<KioskDto>>
    {
        public string SortBy { get; set; }

        public List<Guid> KioskIds { get; set; }

        public class GetKioskQueryHandler : IRequestHandler<GetKioskQuery, List<KioskDto>>
        {
            private readonly IApplicationDbContext _context;
            public GetKioskQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<List<KioskDto>> Handle(GetKioskQuery request, CancellationToken cancellationToken)
            {
                var response = new List<KioskDto>();

                var qry = _context.Kiosks.AsQueryable();

                if (request.KioskIds != null && request.KioskIds.Any())
                {
                    qry = qry.Where(k => request.KioskIds.Contains(k.Id));
                }
                var output = await qry.Select(k => new KioskDto
                {
                    Id = k.Id,
                    KioskName = k.KioskName,
                    Category = k.Category,
                    Status = k.Status
                }).ToListAsync();
                response = output;
                return response;
            }
        }
    }
}
