namespace CovaldysPilot.Application.DTOs.User.Response;

public class CreateUserManuallyResponseDto
{
  public Guid Id { get; set; }
  public required string Pseudo { get; set; }
  public required string Email { get; set; }
  public required string FirstName { get; set; }
  public string? LastName { get; set; }
  public bool IsMembershipUpToDate { get; set; }
  //  UNE SEULE FOIS pour l'admin, jamais stock en clair en DB !!!!!
  public required string TemporaryPassword { get; set; }
}