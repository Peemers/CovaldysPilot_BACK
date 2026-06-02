using CovaldysPilot.Application.DTOs.User.Response;
using CovaldysPilot.Domain.Entities;

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
}