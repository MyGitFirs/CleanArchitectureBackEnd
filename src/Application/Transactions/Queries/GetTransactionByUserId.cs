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
    public class GetTransactionByUserId : IRequest<List<TransactionDto>>
    {
        public Guid? UserId { get; set; } // Added for filtering by User
        public string SortBy { get; set; }
        public List<Guid> TransactionIds { get; set; }

        public class GetTransactionByUserIdHandler : IRequestHandler<GetTransactionByUserId, List<TransactionDto>>
        {
            private readonly IApplicationDbContext _context;

            public GetTransactionByUserIdHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<List<TransactionDto>> Handle(GetTransactionByUserId request, CancellationToken cancellationToken)
            {
                var qry = _context.Transactions
                    .Include(t => t.User)
                    .Include(t => t.Kiosk)
                    .AsQueryable();

                if (request.UserId.HasValue)
                {
                    qry = qry.Where(t => t.UserId == request.UserId.Value);
                }

                if (request.TransactionIds != null && request.TransactionIds.Any())
                {
                    qry = qry.Where(t => request.TransactionIds.Contains(t.Id));
                }

                // Sorting logic
                if (!string.IsNullOrEmpty(request.SortBy))
                {
                    switch (request.SortBy.ToLower())
                    {
                        case "date_asc":
                            qry = qry.OrderBy(t => t.CreatedDate);
                            break;
                        case "date_desc":
                            qry = qry.OrderByDescending(t => t.CreatedDate);
                            break;
                    }
                }
                else
                {
                    qry = qry.OrderByDescending(t => t.CreatedDate);
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
                    Amount = t.Amount,
                    CreatedDate = t.CreatedDate
                }).ToListAsync(cancellationToken);

                return output;
            }
        }
    }
}
