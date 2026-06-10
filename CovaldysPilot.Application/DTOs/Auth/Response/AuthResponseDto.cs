namespace CovaldysPilot.Application.DTOs.Auth.Response;

public class AuthResponseDto
{
  public required Guid UserId { get; set; }
  public required string AccessToken { get; set; }
  public required string RefreshToken { get; set; }
  public required string Pseudo { get; set; }
  public required string Role { get; set; }
  public required DateTime ExpiresAt { get; set; }
  public required string FirstName { get; set; }
  public string? LastName { get; set; }
  
  public bool IsMembershipUpToDate { get; set; }
}