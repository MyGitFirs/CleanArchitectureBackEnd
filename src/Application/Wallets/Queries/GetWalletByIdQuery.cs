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
    public class GetWalletByIdQuery : IRequest<WalletDto>
    {
        public Guid Id { get; set; }

        public class GetWalletByIdQueryHandler : IRequestHandler<GetWalletByIdQuery, WalletDto>
        {
            private readonly IApplicationDbContext _context;

            public GetWalletByIdQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<WalletDto> Handle(GetWalletByIdQuery request, CancellationToken cancellationToken)
            {
                var wallet = await _context.Wallets
                    .Where(w => w.Id == request.Id)
                    .Select(w => new WalletDto
                    {
                        Id = w.Id,
                        UserId = w.UserID,
                        Balance = w.Balance,
                        MonthlyLimit = w.MonthlyLimit,
                        LastResetDate = w.LastResetDate
                    })
                    .FirstOrDefaultAsync(cancellationToken);

                return wallet ?? throw new KeyNotFoundException("Wallet not found");
            }
        }
    }
}
