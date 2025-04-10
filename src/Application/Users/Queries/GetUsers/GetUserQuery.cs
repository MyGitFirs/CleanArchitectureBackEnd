using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.User._Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.User.Queries.GetUsers
{
    public class GetUserQuery: IRequest<List<UserDto>>
    {
        public string SortBy { get; set; }

        public List<Guid> UserIds { get; set; }

        public class GetUserQueryHandler : IRequestHandler<GetUserQuery, List<UserDto>>
        {
            private readonly IApplicationDbContext _context;
            public GetUserQueryHandler(IApplicationDbContext context) {
                _context = context;
            }
            public async Task<List<UserDto>> Handle(GetUserQuery request, CancellationToken cancellationToken)
            {
                var response = new List<UserDto>();

                var qry = _context.Users.AsQueryable();

                if (request.UserIds != null && request.UserIds.Any()) 
                {
                    qry = qry.Where(up => request.UserIds.Contains(up.Id));
                }
                var output = await qry.Select(up => new UserDto{
                    Id = up.Id,
                    FirstName = up.FirstName,
                    LastName = up.LastName,
                    Role = up.Role,
                    Email = up.Email}).ToListAsync();
                response = output;
                return response;
            }
        }
    }
}
