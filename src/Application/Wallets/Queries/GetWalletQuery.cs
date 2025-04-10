using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Wallets._Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Wallets.Queries.GetWallets
{
    public class GetWalletQuery : IRequest<List<WalletDto>>
    {
        public string SortBy { get; set; }
        public List<Guid> WalletIds { get; set; }

        public class GetWalletQueryHandler : IRequestHandler<GetWalletQuery, List<WalletDto>>
        {
            private readonly IApplicationDbContext _context;

            public GetWalletQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<List<WalletDto>> Handle(GetWalletQuery request, CancellationToken cancellationToken)
            {
                var response = new List<WalletDto>();

                var qry = _context.Wallets.AsQueryable();

                if (request.WalletIds != null && request.WalletIds.Any())
                {
                    qry = qry.Where(w => request.WalletIds.Contains(w.Id));
                }

                var output = await qry.Select(w => new WalletDto
                {
                    Id = w.Id,
                    UserId = w.UserID,
                    Balance = w.Balance,
                    MonthlyLimit = w.MonthlyLimit,
                    LastResetDate = w.LastResetDate
                }).ToListAsync(cancellationToken);

                response = output;
                return response;
            }
        }
    }
}
