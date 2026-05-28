using CovaldysPilot.Application.DTOs.SignIn.Request;
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

  public static SignIn ToSignIn(this CreateSignInRequestDto dto, Guid userId, bool isOnWaitingList)
  {
    return new SignIn
    {
      UserId = userId,
      EventId = dto.EventId,
      RegistrationDate = DateTime.Now,
      IsOnWaitingList = isOnWaitingList,
      IsPaymentValid = false,
      CreatedAt = DateTime.Now,
    };
  }
}