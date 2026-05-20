namespace CovaldysPilot.Application.DTOs.Auth.Request;

public class LoginRequestDto
{
  public required string EmailOrPseudo { get; set; }
  public required string Password { get; set; }
}