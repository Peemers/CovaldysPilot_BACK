using CovaldysPilot.Application.DTOs.SignIn.Request;
using CovaldysPilot.Application.DTOs.SignIn.Response;
using CovaldysPilot.Application.Interfaces.Repositories;
using CovaldysPilot.Application.Interfaces.Services;
using CovaldysPilot.Application.Mappers;
using CovaldysPilot.Domain.Entities;
using CovaldysPilot.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace CovaldysPilot.Application.Services;

public class SignInService(
  ISignInRepository signInRepository,
  IEventRepository eventRepository,
  ILogger<SignInService> logger) : ISignInService
{
  #region RegisterSignIn

  public async Task<SignInResponseDto> RegisterAsync(Guid userId, CreateSignInRequestDto dto)
  {
    logger.LogInformation("Inscription de {UserId} à l'événement {EventId}", userId, dto.EventId);

    //recup event
    Event? evt = await eventRepository.GetByIdWithDetailsAsync(dto.EventId);
    if (evt == null)
      throw new KeyNotFoundException($"Événement {dto.EventId} introuvable.");

    //regle event en attente
    if (evt.Status != EventStatus.EnAttente)
    {
      throw new InvalidOperationException("Cet événement n'accepte plus d'incritpions.");
    }

    //regle date limite dépassée ?
    if (evt.RegistrationDeadline < DateTime.UtcNow)
    {
      throw new InvalidOperationException("La date limite d'inscription est dépassée.");
    }

    //regle deja inscrit ?
    SignIn? existing = await signInRepository.GetByUserAndEventAsync(userId, dto.EventId);
    if (existing != null)
    {
      throw new InvalidOperationException("Vous êtes deja inscrit à cet événement.");
    }

    //regle place dispo et liste d'attente
    int currentParticipants = await eventRepository.GetCurrentParticipantsCountAsync(dto.EventId);
    bool isFull = currentParticipants >= evt.MaxParticipants;
    if (isFull && !evt.IsWaitingListActive)
    {
      throw new InvalidOperationException("L'événement est complet et ne dispose pas de file d'attente");
    }
    
    SignIn signIn = dto.ToSignIn(userId, isFull);
    
    if (isFull)
    {
      int waitingCount = await signInRepository.GetWaitingListCountAsync(dto.EventId);
      signIn.WaitingListPosition = waitingCount + 1;
    }

    await signInRepository.AddAsync(signIn);
    await signInRepository.SaveChangesAsync();
    
    logger.LogInformation("Inscription créée — EnAttente: {IsWaiting}", signIn.IsOnWaitingList);
    return signIn.ToSignInResponseDto();
  }

  #endregion

  public async Task UnregisterAsync(Guid userId, Guid signInId)
  {
    //verif inscription
    
    //verif si bien celle de userid
    
    //verif si event est avec status .enAttente
    
    // si pas en liste d'attente -> proouvoir le premier en attente via la methode plus bas
  }

  private async Task PromoteFirstOnWaitingListAsync(Guid eventId)
  {
    SignIn? firstOnWaiting = await signInRepository.GetFirstOnWaitingListAsync(eventId);
    if (firstOnWaiting == null) return;

    // Promouvoir vers inscription confirmee et sortie de la WL
    firstOnWaiting.IsOnWaitingList = false;
    firstOnWaiting.WaitingListPosition = null;

    await signInRepository.UpdateAsync(firstOnWaiting);
    await signInRepository.SaveChangesAsync();

    logger.LogInformation("Membre {UserId} promu depuis la liste d'attente", firstOnWaiting.UserId);
  }
}