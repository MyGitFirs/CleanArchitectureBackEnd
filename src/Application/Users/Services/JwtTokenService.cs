using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using CleanArchitecture.Domain.Entities;

public interface IJwtTokenService
{
    string GenerateToken(User user);
    string GenerateRefreshToken();
}

public class JwtTokenService : IJwtTokenService
{
    private readonly string _jwtSecret = "k5IGi0NDpIq3E4UHxTncw-wR_pSo6_7Hw5XVECzvwCM="; // Move to configuration
    private readonly int _jwtLifespan = 15; // Access Token expiration in minutes
    private readonly int _refreshTokenLifespan = 7; // Refresh Token expiration in days

    public string GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSecret);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            }),
            Expires = DateTime.UtcNow.AddMinutes(_jwtLifespan),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
    }
}
