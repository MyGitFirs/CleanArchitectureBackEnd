using System;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Interfaces;
using MediatR;
using BCrypt.Net;

namespace CleanArchitecture.Application.User.Commands
{
    public class CreateUserCommand : IRequest<string>
    {
        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public string UserNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public class CreateUserCmdHandler : IRequestHandler<CreateUserCommand, string>
        {
            private readonly IApplicationDbContext _context;

            public CreateUserCmdHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<string> Handle(CreateUserCommand request, CancellationToken cancellationToken)
            {
                var user = new CleanArchitecture.Domain.Entities.User
                {
                    Id = Guid.NewGuid(),
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    Role = request.Role,
                    UserNumber = request.UserNumber,
                    Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync(cancellationToken);

                return user.Id.ToString();
            }
        }
    }
}
