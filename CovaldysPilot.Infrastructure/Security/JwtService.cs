using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CovaldysPilot.Application.Interfaces.Services;
using CovaldysPilot.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CovaldysPilot.Infrastructure.Security;

public class JwtService(IConfiguration configuration) : IJwtService
{
  public string GenerateAccessToken(User user)
  {
    var jwtSettings = configuration.GetSection("JwtSettings");
    var secret = jwtSettings["Secret"]!;
    var issuer = jwtSettings["Issuer"]!;
    var audience = jwtSettings["Audience"]!;
    var expiryMinutes = int.Parse((string)jwtSettings["ExpiryMinutes"]!);

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes((string)secret));
    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var claims = new[]
    {
      new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
      new Claim(JwtRegisteredClaimNames.Email, user.Email),
      new Claim("role", user.Role.ToString()),
      new Claim("pseudo", user.Pseudo),
      new Claim("firstname", user.FirstName),
      new Claim("lastname", user.LastName ?? ""),
      new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

    var token = new JwtSecurityToken(
      issuer: issuer,
      audience: audience,
      claims: claims,
      expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
      signingCredentials: credentials
    );

    return new JwtSecurityTokenHandler().WriteToken(token);
  }

  public string GenerateRefreshToken()
  {
    var randomBytes = new byte[64];
    using var rng = RandomNumberGenerator.Create();
    rng.GetBytes(randomBytes);
    return Convert.ToBase64String(randomBytes);
  }

  public DateTime GetRefreshTokenExpiryDate()
  {
    var days = int.Parse(configuration["JwtSettings:RefreshTokenExpiryDays"]!);
    return DateTime.UtcNow.AddDays(days);
  }
}