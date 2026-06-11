using CovaldysPilot.Domain.Enums;

namespace CovaldysPilot.Application.DTOs.User.Request;

public class CreateUserManuallyRequestDto
{
  public required string FirstName { get; set; }
  public required string LastName { get; set; }
  public required string Email { get; set; }
  public required string Pseudo { get; set; }
  public required DateTime Birthday { get; set; }
  public required Genre Gender { get; set; }
  public bool IsMembershipUpToDate { get; set; } = false;
}