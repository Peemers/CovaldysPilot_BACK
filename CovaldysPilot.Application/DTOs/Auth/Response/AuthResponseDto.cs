namespace CovaldysPilot.Application.DTOs.Auth.Response;

public class AuthResponseDto
{
  public required Guid UserId { get; init; }
  public required string AccessToken { get; init; }
  public required string RefreshToken { get; init; }
  public required string Pseudo { get; init; }
  public required string Role { get; init; }
  public required DateTime ExpiresAt { get; init; }
  public required string FirstName { get; init; }
  public string? LastName { get; init; }
  public bool IsMembershipUpToDate { get; init; }
}