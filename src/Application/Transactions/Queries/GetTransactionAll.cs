using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Transactions._Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Transactions.Queries.GetTransactionAll
{
    public class GetTransactionAllQuery : IRequest<List<TransactionReportDto>>
    {
        public class GetTransactionAllQueryHandler : IRequestHandler<GetTransactionAllQuery, List<TransactionReportDto>>
        {
            private readonly IApplicationDbContext _context;

            public GetTransactionAllQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<List<TransactionReportDto>> Handle(GetTransactionAllQuery request, CancellationToken cancellationToken)
            {
                var report = await _context.Transactions
                    .Include(t => t.Kiosk)  
                    .Select(t => new TransactionReportDto
                    {
                        KioskName = t.Kiosk.KioskName,
                        Category = t.Kiosk.Category,
                        CreatedDate = t.CreatedDate,  
                        Amount = t.Amount
                    })
                    .ToListAsync(cancellationToken);

                return report;
            }
        }
    }
}
