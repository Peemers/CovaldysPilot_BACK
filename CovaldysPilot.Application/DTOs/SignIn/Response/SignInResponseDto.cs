namespace CovaldysPilot.Application.DTOs.SignIn.Response;

public class SignInResponseDto
{
  public Guid Id { get; set; }
  public Guid EventId { get; set; }
  public Guid UserId { get; set; }
  public DateTime RegistrationDate { get; set; }
  public bool IsOnWaitingList { get; set; }
  public int? WaitingListPosition { get; set; }
  public bool IsPaymentValid { get; set; }
  public string? UserPseudo { get; set; }
  public string? UserFirstName { get; set; }
  public string? UserLastName { get; set; }
}