using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Transactions._Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Transactions.Queries.GetTransactions
{
    public class GetTransactionQuery : IRequest<List<TransactionDto>>
    {
        public string SortBy { get; set; }
        public List<Guid> TransactionIds { get; set; }

        public class GetTransactionQueryHandler : IRequestHandler<GetTransactionQuery, List<TransactionDto>>
        {
            private readonly IApplicationDbContext _context;

            public GetTransactionQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<List<TransactionDto>> Handle(GetTransactionQuery request, CancellationToken cancellationToken)
            {
                var response = new List<TransactionDto>();

                var qry = _context.Transactions.AsQueryable();

                if (request.TransactionIds != null && request.TransactionIds.Any())
                {
                    qry = qry.Where(t => request.TransactionIds.Contains(t.Id));
                }

                var output = await qry.Select(t => new TransactionDto
                {
                    Id = t.Id,
                    UserId = t.UserId,
                    FirstName = t.User.FirstName,
                    LastName = t.User.LastName,
                    KioskId = t.KioskId,
                    KioskName = t.Kiosk.KioskName,
                    Category = t.Kiosk.Category,
                    Amount = t.Amount
                }).ToListAsync(cancellationToken);

                response = output;
                return response;
            }
        }
    }
}
