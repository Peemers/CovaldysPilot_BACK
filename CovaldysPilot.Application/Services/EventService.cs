using CovaldysPilot.Application.DTOs.Event.Request;
using CovaldysPilot.Application.DTOs.Event.Response;
using CovaldysPilot.Application.Email.Templates;
using CovaldysPilot.Application.Interfaces.Repositories;
using CovaldysPilot.Application.Interfaces.Services;
using CovaldysPilot.Application.Mappers;
using CovaldysPilot.Domain.Entities;
using CovaldysPilot.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace CovaldysPilot.Application.Services;

public class EventService(
  IEventRepository eventRepository,
  ICategoryRepository categoryRepository,
  ISignInRepository signInRepository,
  IEmailService emailService,
  IUserRepository userRepository,
  ILogger<EventService> logger) : IEventService
{
  #region GetAllAsync
  /// <inheritdoc/>
  public async Task<IEnumerable<EventResponseDto>> GetAllAsync(Guid? currentUserId = null)
  {
    logger.LogInformation("Récupération de tous les événements");
    IEnumerable<Event> events = await eventRepository.GetAllWithCategoriesAsync();

    List<EventResponseDto> result = new List<EventResponseDto>();
    foreach (Event evt in events)
    {
      int currentParticipants = await eventRepository.GetCurrentParticipantsCountAsync(evt.Id);
      (bool canRegister, bool isRegistered, bool isOnWaitingList , Guid? signInId) = CalculateRegistrationStatus(evt, currentUserId, currentParticipants);
      result.Add(evt.ToEventResponseDto(currentParticipants, canRegister, isRegistered, signInId:  signInId, isOnWaitingList: isOnWaitingList));
    }

    return result;
  }
  #endregion

  #region GetByIdAsync
  /// <inheritdoc/>
  public async Task<EventResponseDto?> GetByIdAsync(Guid id, Guid? currentUserId = null)
  {
    logger.LogInformation("Récupération de l'événement {Id}", id);
    Event? evt = await eventRepository.GetByIdWithDetailsAsync(id);
    if (evt == null) return null;

    int currentParticipants = await eventRepository.GetCurrentParticipantsCountAsync(id);
    (bool canRegister, bool isRegistered, bool isOnWaitingList, Guid? signInId) = CalculateRegistrationStatus(evt, currentUserId, currentParticipants);
    int? waitingListPosition = GetWaitingListPosition(evt, currentUserId);

    return evt.ToEventResponseDto(currentParticipants, canRegister, isRegistered, waitingListPosition, signInId, isOnWaitingList);
  }
  #endregion

  #region CreateAsync
  /// <inheritdoc/>
  public async Task<EventResponseDto> CreateAsync(CreateEventRequestDto dto)
  {
    logger.LogInformation("Création d'un événement : {Name}", dto.Name);
    ValidateEventDates(dto.StartDate, dto.EndDate, dto.RegistrationDeadline);
    ValidateParticipants(dto.MinParticipants, dto.MaxParticipants);

    Event evt = dto.ToEvent();
    await LinkCategoriesToEventAsync(evt, dto.CategoryIds);

    await eventRepository.AddAsync(evt);
    await eventRepository.SaveChangesAsync();

    Event? createdEvent = await eventRepository.GetByIdWithDetailsAsync(evt.Id);
    logger.LogInformation("Événement créé : {Name}", dto.Name);

    if (createdEvent == null)
      throw new InvalidOperationException("Erreur lors de la récupération de l'événement créé.");

    return createdEvent.ToEventResponseDto();
  }
  #endregion

  #region UpdateAsync
  /// <inheritdoc/>
  public async Task<EventResponseDto> UpdateAsync(Guid id, UpdateEventRequestDto dto)
  {
    logger.LogInformation("Modification de l'événement {Id}", id);
    Event? evt = await eventRepository.GetByIdWithDetailsAsync(id);

    EnsureEventExists(evt, id);
    EnsureEventStatus(evt!, EventStatus.EnAttente, "modifiés");

    Event validEvt = evt!;

    int currentParticipants = await eventRepository.GetCurrentParticipantsCountAsync(id);
    if (dto.MaxParticipants < currentParticipants)
      throw new InvalidOperationException($"Le nombre maximum ne peut pas être inférieur au nombre d'inscrits ({currentParticipants}).");

    validEvt.UpdateFromDto(dto);

    validEvt.EventCategories.Clear();
    await LinkCategoriesToEventAsync(validEvt, dto.CategoryIds);

    await eventRepository.UpdateAsync(validEvt);
    await eventRepository.SaveChangesAsync();

    logger.LogInformation("Événement modifié : {Id}", id);
    return validEvt.ToEventResponseDto(currentParticipants);
  }
  #endregion

  #region DeleteAsync
  /// <inheritdoc/>
  public async Task DeleteAsync(Guid id)
  {
    logger.LogInformation("Suppression de l'événement {Id}", id);
    Event? evt = await eventRepository.GetByIdAsync(id);

    EnsureEventExists(evt, id);
    EnsureEventStatus(evt!, EventStatus.EnAttente, "supprimés");
    
    IEnumerable<SignIn> signIns = await signInRepository.GetByEventAsync(id);
    foreach (SignIn signIn in signIns)
    {
      await signInRepository.DeleteAsync(signIn.Id);
    }

    await eventRepository.DeleteAsync(id);
    await eventRepository.SaveChangesAsync();
    logger.LogInformation("Événement supprimé : {Id}", id);
  }
  #endregion

  #region CancelAsync
  /// <inheritdoc/>
  public async Task CancelAsync(Guid id, string? cancellationReason = null)
  {
    logger.LogInformation("Annulation de l'événement {Id}", id);
    Event? evt = await eventRepository.GetByIdAsync(id);

    EnsureEventExists(evt, id);

    if (evt!.Status != EventStatus.EnAttente && evt.Status != EventStatus.EnCours)
      throw new InvalidOperationException("Seuls les événements en attente ou en cours peuvent être annulés.");

    evt.Status = EventStatus.Annule;
    evt.CancellationReason = cancellationReason;
    evt.UpdatedAt = DateTime.UtcNow;

    await eventRepository.UpdateAsync(evt);
    await eventRepository.SaveChangesAsync();

    await SendEmailToAllSubscribersAsync(
      id,
      $"Annulation de  l'événement - {evt.Name}",
      user => EmailTemplates.EventCancellation(user.FirstName, evt.Name, evt.StartDate, evt.CancellationReason));

    logger.LogInformation("Événement annulé : {Id}", id);
  }
  #endregion

  #region StartAsync
  /// <inheritdoc/>
  public async Task StartAsync(Guid id)
  {
    logger.LogInformation("Démarrage de l'événement {Id}", id);
    Event? evt = await eventRepository.GetByIdAsync(id);

    EnsureEventExists(evt, id);
    EnsureEventStatus(evt!, EventStatus.EnAttente, "démarrés");

    int currentParticipants = await eventRepository.GetCurrentParticipantsCountAsync(id);
    if (currentParticipants < evt!.MinParticipants)
      throw new InvalidOperationException($"Le nombre minimum de participants ({evt.MinParticipants}) n'est pas atteint.");
    if (evt.StartDate > DateTime.UtcNow)
      throw new InvalidOperationException("La date de début n'est pas encore atteinte.");

    evt.Status = EventStatus.EnCours;
    evt.UpdatedAt = DateTime.UtcNow;

    await eventRepository.UpdateAsync(evt);
    await eventRepository.SaveChangesAsync();
    logger.LogInformation("Événement démarré : {Id}", id);
  }
  #endregion

  #region CloseAsync
  /// <inheritdoc/>
  public async Task CloseAsync(Guid id)
  {
    logger.LogInformation("Clôture de l'événement {Id}", id);
    Event? evt = await eventRepository.GetByIdAsync(id);

    EnsureEventExists(evt, id);
    EnsureEventStatus(evt!, EventStatus.EnCours, "clôturés");

    evt!.Status = EventStatus.Termine;
    evt.UpdatedAt = DateTime.UtcNow;

    await eventRepository.UpdateAsync(evt);
    await eventRepository.SaveChangesAsync();
    logger.LogInformation("Événement clôturé : {Id}", id);
  }
  #endregion

  #region GetStatsAsync
  /// <inheritdoc/>
  public async Task<EventStatsResponseDto> GetStatsAsync(Guid id)
  {
    logger.LogInformation("Récupération des stats de l'événement : {Id}", id);

    Event? evt = await eventRepository.GetByIdAsync(id);
    if (evt is null)
      throw new KeyNotFoundException($"Événement {id} introuvable.");

    int confirmed = await eventRepository.GetCurrentParticipantsCountAsync(id);
    int waiting = await signInRepository.GetWaitingListCountAsync(id);

    return evt.ToEventStatsResponseDto(confirmed, waiting);
  }
  #endregion
  
  #region SendReminderAsync
  /// <inheritdoc/>
  public async Task SendReminderAsync(Guid id)
  {
    logger.LogInformation("Envoi d'un rappel pour l'événement {Id}", id);

    Event? evt = await eventRepository.GetByIdAsync(id);
    if (evt is null)
      throw new KeyNotFoundException($"Événement {id} introuvable.");

    await SendEmailToAllSubscribersAsync(
      id,
      $"Rappel — {evt.Name}",
      user => EmailTemplates.EventReminder(user.FirstName, evt.Name, evt.StartDate, evt.Location)
    );

    logger.LogInformation("Rappel envoyé pour l'événement {Id}", id);
  }
  #endregion
  
  #region UpdateCoverImageAsync
  /// <inheritdoc/>
  public async Task UpdateCoverImageAsync(Guid id, string imageUrl)
  {
    logger.LogInformation("Mise à jour de l'image de couverture de l'événement {Id}", id);
    Event? evt = await eventRepository.GetByIdAsync(id);
    
    EnsureEventExists(evt, id);
    
    evt!.CoverImage = imageUrl;
    evt.UpdatedAt = DateTime.UtcNow;
    
    await eventRepository.UpdateAsync(evt);
    await eventRepository.SaveChangesAsync();
    logger.LogInformation("Image de couverture mise à jour : {Id}", id);
  }
  #endregion

  //Methodes privées

  #region EnsureEventExists
  /// <summary>
  /// Vérifie si l'événement existe.
  /// </summary>
  /// <param name="evt">L'événement à vérifier.</param>
  /// <param name="id">L'identifiant unique de l'événement.</param>
  /// <exception cref="KeyNotFoundException">Levée si l'événement est nul.</exception>
  private static void EnsureEventExists(Event? evt, Guid id)
  {
    if (evt == null)
      throw new KeyNotFoundException($"Événement {id} introuvable.");
  }
  #endregion

  #region EnsureEventStatus
  /// <summary>
  /// Vérifie si le statut de l'événement correspond au statut attendu pour effectuer une action.
  /// </summary>
  /// <param name="evt">L'événement à vérifier.</param>
  /// <param name="expected">Le statut attendu.</param>
  /// <param name="action">L'action effectuée.</param>
  /// <exception cref="InvalidOperationException">Levée si le statut ne correspond pas.</exception>
  private static void EnsureEventStatus(Event evt, EventStatus expected, string action)
  {
    if (evt.Status != expected)
      throw new InvalidOperationException($"Seuls les événements en statut '{expected}' peuvent être {action}.");
  }
  #endregion

  #region ValidateEventDates
  /// <summary>
  /// Valide la cohérence des dates d'un événement.
  /// </summary>
  /// <param name="startDate">La date de début de l'événement.</param>
  /// <param name="endDate">La date de fin de l'événement.</param>
  /// <param name="registrationDeadline">La date limite d'inscription.</param>
  /// <exception cref="InvalidOperationException">Levée si les dates ne sont pas cohérentes.</exception>
  private static void ValidateEventDates(DateTime startDate, DateTime endDate, DateTime registrationDeadline)
  {
    if (startDate <= DateTime.UtcNow)
      throw new InvalidOperationException("La date de début doit être postérieure à aujourd'hui.");
    if (endDate <= startDate)
      throw new InvalidOperationException("La date de fin doit être postérieure à la date de début.");
    if (registrationDeadline > startDate)
      throw new InvalidOperationException("La date limite d'inscription doit être antérieure à la date de début.");
  }
  #endregion

  #region ValidateParticipants
  /// <summary>
  /// Valide les limites du nombre de participants.
  /// </summary>
  /// <param name="min">Le nombre minimum de participants.</param>
  /// <param name="max">Le nombre maximum de participants.</param>
  /// <exception cref="InvalidOperationException">Levée si le minimum est supérieur au maximum.</exception>
  private static void ValidateParticipants(int min, int max)
  {
    if (min > max)
      throw new InvalidOperationException("Le nombre minimum doit être inférieur ou égal au maximum.");
  }
  #endregion

  #region CalculateRegistrationStatus
  /// <summary>
  /// Calcule l'état d'inscription d'un utilisateur connecté pour un événement.
  /// </summary>
  /// <param name="evt">L'événement concerné.</param>
  /// <param name="currentUserId">L'identifiant de l'utilisateur actuel.</param>
  /// <param name="currentParticipants">Le nombre actuel de participants confirmés.</param>
  /// <returns>Un tuple contenant les indicateurs d'inscription.</returns>
  private static (bool canRegister, bool isRegistered, bool isOnWaitingList, Guid? signInId) CalculateRegistrationStatus(Event evt, Guid? currentUserId, int currentParticipants)
  {
    if (!currentUserId.HasValue)
      return (false, false, false, null);
    
    SignIn? confirmedSignIn = evt.SignIns.FirstOrDefault(s => s.UserId == currentUserId.Value && !s.IsOnWaitingList);
    SignIn? waitingSignIn = evt.SignIns.FirstOrDefault(s => s.UserId == currentUserId.Value && s.IsOnWaitingList);

    bool isRegistered = confirmedSignIn != null;
    bool isOnWaitingList = waitingSignIn != null;

    bool canRegister = !isRegistered &&
                       !isOnWaitingList &&
                       evt.Status == EventStatus.EnAttente &&
                       evt.RegistrationDeadline >= DateTime.UtcNow &&
                       (currentParticipants < evt.MaxParticipants);

    Guid? signInId = confirmedSignIn?.Id ?? waitingSignIn?.Id;

    return (canRegister, isRegistered, isOnWaitingList, signInId);
  }
  #endregion

  #region GetWaitingListPosition
  /// <summary>
  /// Récupère la position d'un utilisateur sur la liste d'attente d'un événement.
  /// </summary>
  /// <param name="evt">L'événement concerné.</param>
  /// <param name="currentUserId">L'identifiant de l'utilisateur.</param>
  /// <returns>La position sur la liste d'attente, ou null s'il n'y figure pas.</returns>
  private static int? GetWaitingListPosition(Event evt, Guid? currentUserId)
  {
    if (!currentUserId.HasValue) return null;
    SignIn? signIn = evt.SignIns.FirstOrDefault(s => s.UserId == currentUserId.Value && s.IsOnWaitingList);
    return signIn?.WaitingListPosition;
  }
  #endregion

  #region LinkCategoriesToEventAsync
  /// <summary>
  /// Lie les catégories spécifiées à un événement.
  /// </summary>
  /// <param name="evt">L'événement concerné.</param>
  /// <param name="categoryIds">La liste des identifiants de catégories.</param>
  /// <returns>Une tâche asynchrone représentant l'opération.</returns>
  private async Task LinkCategoriesToEventAsync(Event evt, List<Guid> categoryIds)
  {
    foreach (Guid categoryId in categoryIds)
    {
      Category? category = await categoryRepository.GetByIdAsync(categoryId);
      if (category != null)
      {
        evt.EventCategories.Add(new EventCategory
        {
          EventId = evt.Id,
          CategoryId = categoryId
        });
      }
    }
  }
  #endregion

  #region SendEmailToAllSubscribersAsync
  /// <summary>
  /// Envoie un courriel à tous les participants inscrits à un événement.
  /// </summary>
  /// <param name="eventId">L'identifiant de l'événement.</param>
  /// <param name="subject">L'objet du message.</param>
  /// <param name="buildBody">La fonction générant le corps du message pour chaque utilisateur.</param>
  /// <returns>Une tâche asynchrone représentant l'opération.</returns>
  private async Task SendEmailToAllSubscribersAsync(Guid eventId, string subject, Func<User, string> buildBody)
  {
    IEnumerable<SignIn> signIns = await signInRepository.GetByEventAsync(eventId);
    foreach (SignIn signIn in signIns)
    {
      User? user = await userRepository.GetByIdAsync(signIn.UserId);
      if (user != null)
      {
        await emailService.SendEmail(user.Email, user.FirstName, subject, buildBody(user));
      }
    }
  }
  #endregion
}