namespace CovaldysPilot.Domain.Entities;

public class SignIn : BaseEntity
{
  public required Guid UserId { get; set; }
  public User User { get; set; } = null!;

  public required Guid EventId { get; set; }
  public Event Event { get; set; } = null!;

  public required DateTime RegistrationDate { get; set; }
  public bool IsOnWaitingList { get; set; } = false;
  public int? WaitingListPosition { get; set; }
  public bool IsPaymentValid { get; set; } = false;
}