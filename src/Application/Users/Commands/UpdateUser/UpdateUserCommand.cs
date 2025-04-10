using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.User.Commands.UpdateUser
{
    public class UpdateUserCommand : IRequest
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserNumber { get; set; }
        public string Email { get; set; }

        public class UpdateUsertCommandHandler : IRequestHandler<UpdateUserCommand> 
        {
            private readonly IApplicationDbContext _context;

            public UpdateUsertCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

                if (user == null)
                    throw new Exception("User not found");

                user.FirstName = request.FirstName;
                user.LastName = request.LastName;
                user.UserNumber = request.UserNumber;
                user.Email = request.Email;
                user.UpdatedDate = DateTime.Now;

                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }

        }

    }
}
