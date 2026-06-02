using CovaldysPilot.Domain.Enums;

namespace CovaldysPilot.Application.DTOs.User.Response;

public class UserResponseDto
{
  public Guid Id { get; set; }
  public required string Pseudo { get; set; }
  public required string Email { get; set; }
  public required string FirstName { get; set; }
  public string? LastName { get; set; }
  public string? PhoneNumber { get; set; }
  public Role Role { get; set; }
  public Genre Gender { get; set; }
  public DateTime Birthday { get; set; }
  public bool IsMembershipUpToDate { get; set; }
  public DateTime? LastPayementDate { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime? UpdatedAt { get; set; }
}