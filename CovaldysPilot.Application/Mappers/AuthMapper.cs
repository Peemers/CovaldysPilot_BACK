using CovaldysPilot.Application.DTOs.Auth.Request;
using CovaldysPilot.Application.DTOs.Auth.Response;
using CovaldysPilot.Domain.Entities;
using CovaldysPilot.Domain.Enums;

namespace CovaldysPilot.Application.Mappers;

public static class AuthMapper
{
  public static User ToUser(this RegisterRequestDto dto, string passwordHash)
  {
    return new User
    {
      Pseudo = dto.Pseudo,
      Email = dto.Email,
      FirstName = dto.FirstName,
      LastName = dto.LastName,
      PhoneNumber =  dto.PhoneNumber,
      PasswordHash = passwordHash,
      Role = Role.Membre,
      Birthday = dto.Birthday,
      CreatedAt = DateTime.UtcNow,
    };
  }

  // User + tokens → AuthResponseDto
  public static AuthResponseDto ToAuthResponseDto(this User user, string accessToken, string refreshToken, DateTime expiresAt)
  {
    return new AuthResponseDto
    {
      UserId = user.Id,
      AccessToken = accessToken,
      RefreshToken = refreshToken,
      Pseudo = user.Pseudo,
      Role = user.Role.ToString(),
      ExpiresAt = expiresAt,
      FirstName =  user.FirstName,
      LastName =  user.LastName,
      IsMembershipUpToDate = user.IsMembershipUpToDate
    };
  }

  // Créer un RefreshToken entity
  public static RefreshToken ToRefreshTokenEntity(this User user, string token, DateTime expiryDate)
  {
    return new RefreshToken
    {
      Token = token,
      UserId = user.Id,
      ExpirationDate = expiryDate,
      CreatedAt = DateTime.UtcNow
    };
  }
}