namespace CovaldysPilot.Application.DTOs.Auth.Request;

public class RegisterRequestDto
{
  public required string Pseudo { get; set; }
  public required string Email { get; set; }
  public required string Password { get; set; }
  public required string ConfirmPassword { get; set; }
  public required DateTime Birthday { get; set; }
  public string? Gender { get; set; }
}