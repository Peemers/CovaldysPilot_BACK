using System.ComponentModel.DataAnnotations;

namespace CovaldysPilot.Application.DTOs.SignIn.Request;

public class CreateSignInRequestDto
{
  [Required]
  public required Guid EventId { get; set; }
}