namespace CovaldysPilot.Application.DTOs.User.Response;

public class CreateUserManuallyResponseDto
{
  public Guid Id { get; init; }
  public required string Pseudo { get; init; }
  public required string Email { get; init; }
  public required string FirstName { get; init; }
  public string? LastName { get; init; }
  public bool IsMembershipUpToDate { get; init; }
  //  UNE SEULE FOIS pour l'admin, jamais stock en clair en DB !!!!!
  public required string TemporaryPassword { get; init; }
}