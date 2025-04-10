using System;
using System.Collections.Generic;
using System.Text;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Transactions._Dto;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CleanArchitecture.Application.Transactions.Queries
{
    public class GetTransactionByCompanyId : IRequest<List<TransactionDto>>
    {
        public Guid CompanyId { get; set; }
        public string SortBy { get; set; }

        public class GetTransactionByCompanyIdHandler : IRequestHandler<GetTransactionByCompanyId, List<TransactionDto>>
        {
            private readonly IApplicationDbContext _context;

            public GetTransactionByCompanyIdHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<List<TransactionDto>> Handle(GetTransactionByCompanyId request, CancellationToken cancellationToken)
            {
                var qry = _context.Transactions
                    .Include(t => t.User)
                    .Include(t => t.Kiosk)
                    .Where(t => t.User.CompanyID == request.CompanyId) // Filtering by Company
                    .AsQueryable();

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
