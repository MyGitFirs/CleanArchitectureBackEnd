using MediatR;
using BCrypt.Net;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.User._Dto;
using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;

public class RegisterUserCommand : IRequest<UserDto>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public string Password { get; set; }
}

public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, UserDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IJwtTokenService _jwtTokenService;

    public RegisterUserHandler(IApplicationDbContext context, IJwtTokenService jwtTokenService)
    {
        _context = context;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<UserDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {

        if (_context.Users.Any(u => u.Email == request.Email))
        {
            throw new System.Exception("Email already registered.");
        }

        string userNumber = await GenerateUniqueUserNumber();

        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var user = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Role = request.Role,
            UserNumber = userNumber,
            Email = request.Email,
            Password = hashedPassword,
            RefreshToken = _jwtTokenService.GenerateRefreshToken(),
            RefreshTokenExpiry = DateTime.UtcNow.AddDays(7)
        };
        user.RefreshToken = _jwtTokenService.GenerateRefreshToken();
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);

        var token = _jwtTokenService.GenerateToken(user);

        return new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Role = user.Role,
            Email = user.Email,
            Token = token,
            RefreshToken = user.RefreshToken
        };
    }

    private async Task<string> GenerateUniqueUserNumber()
    {

        var userNumbers = await _context.Users
            .Select(u => u.UserNumber)
            .ToListAsync();

        int lastNumber = userNumbers
            .Select(u => int.TryParse(u.Replace("USER-", ""), out int num) ? num : 0)
            .DefaultIfEmpty(0)
            .Max();

        return $"USER-{(lastNumber + 1):D4}";
    }
}