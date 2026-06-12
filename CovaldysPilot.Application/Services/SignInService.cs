using CovaldysPilot.Application.DTOs.SignIn.Request;
using CovaldysPilot.Application.DTOs.SignIn.Response;
using CovaldysPilot.Application.Email.Templates;
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
  IEmailService emailService,
  IUserRepository userRepository,
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
    
    User? user = await userRepository.GetByIdAsync(userId);
    if (user != null)
    {
      await emailService.SendEmail(
        user.Email,
        user.FirstName,
        $"Confirmation d'inscription à l'événement {evt.Name}",
        EmailTemplates.RegistrationConfirmation(user.FirstName, evt.Name, evt.StartDate, evt.Location)
      );
    }
    return signIn.ToSignInResponseDto();
  }

  #endregion

  #region UnregisterSignIn

  public async Task UnregisterAsync(Guid userId, Guid signInId)
  {
    //verif inscription
    SignIn? signIn = await signInRepository.GetByIdAsync(signInId);
    if (signIn == null)
    {
      throw new KeyNotFoundException($"l'inscription {signInId} n'existe pas.");
    }

    //verif si bien celle de userid
    if (signIn.UserId != userId)
    {
      throw new InvalidOperationException("Vous ne pouvez pas vous désinscrire d'un événement appartenant à un autre utilisateur.");
    }

    //verif si event est avec status .enAttente
    Event? evt = await eventRepository.GetByIdAsync(signIn.EventId);
    if (evt == null)
    {
      throw new KeyNotFoundException($"L'événement {signIn.EventId} est introuvable");
    }

    if (evt.Status != EventStatus.EnAttente)
    {
      throw new InvalidOperationException("Vous ne pouvez vous désinscrire d'un événement qui n'est pas en attente");
    }

    bool wasOnWaitingList = signIn.IsOnWaitingList;

    await signInRepository.DeleteAsync(signInId);
    await signInRepository.SaveChangesAsync();

    // si le désinscrit n'était pas dans la liste -> proouvoir le premier en attente via la methode plus bas

    if (!wasOnWaitingList)
    {
      await PromoteFirstOnWaitingListAsync(signIn.EventId);
    }

    logger.LogInformation("Désinscription effectuée : {signInId}", signInId);
  }

  #endregion

  #region GetByEvent

  public async Task<IEnumerable<SignInResponseDto>> GetByEventAsync(Guid eventId)
  {
    IEnumerable<SignIn> signIns = await signInRepository.GetByEventAsync(eventId);
    return signIns.Select(signIn => signIn.ToSignInResponseDto());
  }

  #endregion

  #region GetByUser

  public async Task<IEnumerable<SignInResponseDto>> GetByUserAsync(Guid userId)
  {
    IEnumerable<SignIn> signIns = await signInRepository.GetByUserAsync(userId);
    return signIns.Select(signIn => signIn.ToSignInResponseDto());
  }

  #endregion

  #region PromoteFirstOnWaitingListAsync

  //Methode prive de verif, le premier en fil d'attente gagne sa place dans la file
  
  private async Task PromoteFirstOnWaitingListAsync(Guid eventId)
  {
    SignIn? firstOnWaiting = await signInRepository.GetFirstOnWaitingListAsync(eventId); //repo
    if (firstOnWaiting == null) return;

    // Promouvoir vers inscription confirmee et sortie de la WL
    firstOnWaiting.IsOnWaitingList = false;
    firstOnWaiting.WaitingListPosition = null;

    await signInRepository.UpdateAsync(firstOnWaiting);
    await signInRepository.SaveChangesAsync();

    logger.LogInformation("Membre {UserId} promu depuis la liste d'attente", firstOnWaiting.UserId);
    
    User? user = await userRepository.GetByIdAsync(firstOnWaiting.UserId);
    Event? evt = await eventRepository.GetByIdAsync(eventId);
    if (user != null && evt != null)
    {
      await emailService.SendEmail(
        user.Email,
        user.FirstName,
        $"Bonne nouvelle — Place confirmée pour {evt.Name} !",
        EmailTemplates.WaitingListPromotion(user.FirstName, evt.Name, evt.StartDate, evt.Location)
      );
    }
  }

  #endregion

  #region ValidatePayment

  public async Task ValidatePayment(Guid signInId)
  {
    logger.LogInformation("Validation du paiement pour l'inscription {SignInId}", signInId);

    SignIn? signIn = await signInRepository.GetByIdAsync(signInId);
    if (signIn == null)
      throw new KeyNotFoundException($"Inscription {signInId} introuvable.");

    signIn.IsPaymentValid = true;

    await signInRepository.UpdateAsync(signIn);
    await signInRepository.SaveChangesAsync();

    logger.LogInformation("Paiement validé pour l'inscription {SignInId}", signInId);
  }

  #endregion

  #region AdminRegister

  public async Task<SignInResponseDto> AdminRegisterAsync(Guid userId, Guid eventId)
  {
    logger.LogInformation("Inscription manuelle admin — UserId: {UserId}, EventId: {EventId}", userId, eventId);

    Event? evt = await eventRepository.GetByIdWithDetailsAsync(eventId);
    if (evt == null)
      throw new KeyNotFoundException($"Événement {eventId} introuvable.");

    if (evt.Status != EventStatus.EnAttente)
      throw new InvalidOperationException("Cet événement n'accepte plus d'inscriptions.");

    SignIn? existing = await signInRepository.GetByUserAndEventAsync(userId, eventId);
    if (existing != null)
      throw new InvalidOperationException("Ce membre est déjà inscrit à cet événement.");

    int currentParticipants = await eventRepository.GetCurrentParticipantsCountAsync(eventId);
    bool isFull = currentParticipants >= evt.MaxParticipants;

    if (isFull && !evt.IsWaitingListActive)
      throw new InvalidOperationException("L'événement est complet et ne dispose pas de file d'attente.");
    //?? déplacer dans le mapper
    SignIn signIn = SignInMapper.ToAdminSignIn(userId, eventId, isFull);

    if (isFull)
    {
      int waitingCount = await signInRepository.GetWaitingListCountAsync(eventId);
      signIn.WaitingListPosition = waitingCount + 1;
    }

    await signInRepository.AddAsync(signIn);
    await signInRepository.SaveChangesAsync();

    logger.LogInformation("Inscription manuelle créée — EnAttente: {IsWaiting}", signIn.IsOnWaitingList);
    return signIn.ToSignInResponseDto();
  }

  #endregion

  #region AdminUnregister

  public async Task AdminUnregisterAsync(Guid signInId)
  {
    logger.LogInformation("Désinscription manuelle admin — SignInId: {SignInId}", signInId);

    SignIn? signIn = await signInRepository.GetByIdAsync(signInId);
    if (signIn == null)
      throw new KeyNotFoundException($"Inscription {signInId} introuvable.");

    Event? evt = await eventRepository.GetByIdAsync(signIn.EventId);
    if (evt == null)
      throw new KeyNotFoundException($"Événement {signIn.EventId} introuvable.");

    if (evt.Status != EventStatus.EnAttente)
      throw new InvalidOperationException("Impossible de désinscrire depuis un événement qui n'est pas en attente.");

    bool wasOnWaitingList = signIn.IsOnWaitingList;

    await signInRepository.DeleteAsync(signInId);
    await signInRepository.SaveChangesAsync();

    if (!wasOnWaitingList)
      await PromoteFirstOnWaitingListAsync(signIn.EventId);

    logger.LogInformation("Désinscription manuelle effectuée : {SignInId}", signInId);
  }

  #endregion
}