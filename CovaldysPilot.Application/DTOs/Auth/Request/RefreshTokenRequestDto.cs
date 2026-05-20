using System.ComponentModel.DataAnnotations;

namespace CovaldysPilot.Application.DTOs.Auth.Request;

public class RefreshTokenRequestDto
{
  [Required(ErrorMessage = "Le refresh token est obligatoire.")]
  public required string RefreshToken { get; set; }
}