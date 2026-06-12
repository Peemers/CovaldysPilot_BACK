using CovaldysPilot.Application.DTOs.Event.Response;
using CovaldysPilot.Domain.Enums;

namespace CovaldysPilot.Application.DTOs.SignIn.Response;

public class SignInResponseDto
{
  public Guid Id { get; init; }
  public Guid EventId { get; init; }
  public Guid UserId { get; init; }
  public DateTime RegistrationDate { get; init; }
  public bool IsOnWaitingList { get; init; }
  public int? WaitingListPosition { get; init; }
  public bool IsPaymentValid { get; init; }
  public string? UserPseudo { get; init; }
  public string? UserFirstName { get; init; }
  public string? UserLastName { get; init; }
  public string? EventName { get; init; }
  public EventStatus? EventStatus { get; init; }
  public DateTime? EventStartDate { get; init; }
}