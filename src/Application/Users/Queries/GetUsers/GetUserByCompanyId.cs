using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.User._Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.User.Queries.GetUsers
{
    public class GetUsersByCompanyIdQuery : IRequest<List<UserDto>>
    {
        public Guid CompanyId { get; set; } 

        public class GetUsersByCompanyIdQueryHandler : IRequestHandler<GetUsersByCompanyIdQuery, List<UserDto>>
        {
            private readonly IApplicationDbContext _context;

            public GetUsersByCompanyIdQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<List<UserDto>> Handle(GetUsersByCompanyIdQuery request, CancellationToken cancellationToken)
            {
                var users = await _context.Users
                    .Where(u => u.CompanyID == request.CompanyId) 
                    .Select(u => new UserDto
                    {
                        Id = u.Id,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Email = u.Email
                    })
                    .ToListAsync(cancellationToken);

                return users;
            }
        }
    }
}
