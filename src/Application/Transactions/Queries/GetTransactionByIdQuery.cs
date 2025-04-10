using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Transactions._Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Transactions.Queries.GetTransactionById
{
    public class GetTransactionByIdQuery : IRequest<TransactionDto>
    {
        public Guid Id { get; set; }

        public class GetTransactionByIdQueryHandler : IRequestHandler<GetTransactionByIdQuery, TransactionDto>
        {
            private readonly IApplicationDbContext _context;

            public GetTransactionByIdQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<TransactionDto> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
            {
                var transaction = await _context.Transactions
                    .Include(t => t.User)
                    .Include(t => t.Kiosk)
                    .Where(t => t.Id == request.Id)
                    .Select(t => new TransactionDto
                    {
                        Id = t.Id,
                        UserId = t.UserId,
                        FirstName = t.User.FirstName,
                        LastName = t.User.LastName,
                        KioskId = t.KioskId,
                        KioskName = t.Kiosk.KioskName,
                        Category = t.Kiosk.Category,
                        Amount = t.Amount,
                        CreatedDate = t.CreatedDate,
                    })
                    .FirstOrDefaultAsync(cancellationToken);
                return transaction ?? throw new KeyNotFoundException("Transaction not found");
            }
        }
    }
}