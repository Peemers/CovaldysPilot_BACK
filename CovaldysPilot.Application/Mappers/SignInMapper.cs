using CovaldysPilot.Application.DTOs.SignIn.Response;
using CovaldysPilot.Domain.Entities;

namespace CovaldysPilot.Application.Mappers;

public static class SignInMapper
{
  public static SignInResponseDto ToSignInResponseDto(this SignIn signIn)
  {
    return new SignInResponseDto
    {
      Id = signIn.Id,
      EventId = signIn.EventId,
      UserId = signIn.UserId,
      RegistrationDate = signIn.RegistrationDate,
      IsOnWaitingList = signIn.IsOnWaitingList,
      WaitingListPosition = signIn.WaitingListPosition,
      IsPaymentValid = signIn.IsPaymentValid
    };
  }
}