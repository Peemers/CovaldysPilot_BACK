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
}