using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Transactions._Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Transactions.Queries.GetTransactionReport
{
    public class GetTransactionReport : IRequest<List<TransactionReportDto>>
    {
       
    }

    public class GetTransactionReportQueryHandler : IRequestHandler<GetTransactionReport, List<TransactionReportDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetTransactionReportQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<TransactionReportDto>> Handle(GetTransactionReport request, CancellationToken cancellationToken)
        {

            var report = await _context.Transactions
                 .GroupBy(t => new
                 {
                     t.Kiosk.KioskName,
                     t.Kiosk.Category,
                     CreatedDate = t.CreatedDate.Date
                 })
                 .Select(g => new TransactionReportDto
                 {
                     KioskName = g.Key.KioskName,
                     Category = g.Key.Category,
                     CreatedDate = g.Key.CreatedDate,
                     Amount = g.Sum(t => t.Amount)
                 })
                 .OrderBy(r => r.CreatedDate)
                 .ToListAsync(cancellationToken);

            return report;
        }
    }
}
