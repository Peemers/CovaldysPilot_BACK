using CovaldysPilot.Application.DTOs.User.Request;
using CovaldysPilot.Application.DTOs.User.Response;
using CovaldysPilot.Domain.Entities;
using CovaldysPilot.Domain.Enums;

namespace CovaldysPilot.Application.Mappers;

public static class UserMapper
{
  public static UserResponseDto ToUserResponseDto(this User user)
  {
    return new UserResponseDto
    {
      Id = user.Id,
      Pseudo = user.Pseudo,
      Email = user.Email,
      FirstName = user.FirstName,
      LastName = user.LastName,
      PhoneNumber = user.PhoneNumber,
      Role = user.Role,
      Gender = user.Gender,
      Birthday = user.Birthday,
      IsMembershipUpToDate = user.IsMembershipUpToDate,
      LastPayementDate = user.LastPayementDate,
      CreatedAt = user.CreatedAt,
      UpdatedAt = user.UpdatedAt
    };
  }

  public static User ToUserFromManualCreation(this CreateUserManuallyRequestDto dto, string passwordHash)
  {
    return new User
    {
      FirstName = dto.FirstName,
      LastName = dto.LastName,
      Email = dto.Email,
      Pseudo = dto.Pseudo,
      Birthday = dto.Birthday,
      Gender = dto.Gender,
      IsMembershipUpToDate = dto.IsMembershipUpToDate,
      PasswordHash = passwordHash,
      Role = Role.Membre,
      CreatedAt = DateTime.UtcNow,
      UpdatedAt = DateTime.UtcNow
    };
  }

  public static CreateUserManuallyResponseDto ToCreateUserManuallyResponseDto(this User user, string temporaryPassword)
  {
    return new CreateUserManuallyResponseDto
    {
      Id = user.Id,
      Pseudo = user.Pseudo,
      Email = user.Email,
      FirstName = user.FirstName,
      LastName = user.LastName,
      IsMembershipUpToDate = user.IsMembershipUpToDate,
      TemporaryPassword = temporaryPassword
    };
  }
}