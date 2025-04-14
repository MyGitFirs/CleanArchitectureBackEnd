using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Wallets._Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Wallets.Queries.GetAllWallet
{
    public class GetAllWalletQuery : IRequest<List<WalletUserDto>>
    {

        public string? Search { get; set; }
        public class Handler : IRequestHandler<GetAllWalletQuery, List<WalletUserDto>>
        {

            private readonly IApplicationDbContext _context;

            public Handler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<List<WalletUserDto>> Handle(GetAllWalletQuery request, CancellationToken cancellationToken)
            {
                var query = from wallet in _context.Wallets
                            join user in _context.Users on wallet.UserID equals user.Id
                            select new WalletUserDto
                            {
                                FirstName = user.FirstName,
                                LastName = user.LastName,
                                Balance = wallet.Balance
                            };

                if (!string.IsNullOrWhiteSpace(request.Search))
                {
                    decimal parsedBalance;
                    bool isNumeric = decimal.TryParse(request.Search, out parsedBalance);

                    query = query.Where(w =>
                        w.FirstName.Contains(request.Search) ||
                        w.LastName.Contains(request.Search) ||
                        (isNumeric && w.Balance == parsedBalance)
                    );
                }
                return await query.ToListAsync(cancellationToken);
            }
        }
    }
}
