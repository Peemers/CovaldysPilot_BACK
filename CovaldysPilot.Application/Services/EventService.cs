using CovaldysPilot.Application.DTOs.Event.Request;
using CovaldysPilot.Application.DTOs.Event.Response;
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
  ILogger<EventService> logger) : IEventService
{
  public async Task<IEnumerable<EventResponseDto>> GetAllAsync(Guid? currentUserId = null)
  {
    logger.LogInformation("Récupération de tous les événements");
    IEnumerable<Event> events = await eventRepository.GetAllWithCategoriesAsync();

    List<EventResponseDto> result = new List<EventResponseDto>();
    foreach (Event evt in events)
    {
      int currentParticipants = await eventRepository.GetCurrentParticipantsCountAsync(evt.Id);
      (bool canRegister, bool isRegistered) = CalculateRegistrationStatus(evt, currentUserId, currentParticipants);
      result.Add(evt.ToEventResponseDto(currentParticipants, canRegister, isRegistered));
    }

    return result;
  }

  public async Task<EventResponseDto?> GetByIdAsync(Guid id, Guid? currentUserId = null)
  {
    logger.LogInformation("Récupération de l'événement {Id}", id);
    Event? evt = await eventRepository.GetByIdWithDetailsAsync(id);
    if (evt == null) return null;

    int currentParticipants = await eventRepository.GetCurrentParticipantsCountAsync(id);
    (bool canRegister, bool isRegistered) = CalculateRegistrationStatus(evt, currentUserId, currentParticipants);
    int? waitingListPosition = GetWaitingListPosition(evt, currentUserId);

    return evt.ToEventResponseDto(currentParticipants, canRegister, isRegistered, waitingListPosition);
  }

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

  public async Task<EventResponseDto> UpdateAsync(Guid id, UpdateEventRequestDto dto)
  {
    logger.LogInformation("Modification de l'événement {Id}", id);
    Event? evt = await eventRepository.GetByIdWithDetailsAsync(id);

    EnsureEventExists(evt, id);
    EnsureEventStatus(evt!, EventStatus.EnAttente, "modifiés");

    int currentParticipants = await eventRepository.GetCurrentParticipantsCountAsync(id);
    if (dto.MaxParticipants < currentParticipants)
      throw new InvalidOperationException($"Le nombre maximum ne peut pas être inférieur au nombre d'inscrits ({currentParticipants}).");

    evt!.UpdateFromDto(dto);

    evt.EventCategories.Clear();
    await LinkCategoriesToEventAsync(evt, dto.CategoryIds);

    await eventRepository.UpdateAsync(evt);
    await eventRepository.SaveChangesAsync();

    logger.LogInformation("Événement modifié : {Id}", id);
    return evt.ToEventResponseDto(currentParticipants);
  }

  public async Task DeleteAsync(Guid id)
  {
    logger.LogInformation("Suppression de l'événement {Id}", id);
    Event? evt = await eventRepository.GetByIdAsync(id);

    EnsureEventExists(evt, id);
    EnsureEventStatus(evt!, EventStatus.EnAttente, "supprimés");

    await eventRepository.DeleteAsync(id);
    await eventRepository.SaveChangesAsync();
    logger.LogInformation("Événement supprimé : {Id}", id);
  }

  public async Task CancelAsync(Guid id)
  {
    logger.LogInformation("Annulation de l'événement {Id}", id);
    Event? evt = await eventRepository.GetByIdAsync(id);

    EnsureEventExists(evt, id);

    if (evt!.Status != EventStatus.EnAttente && evt.Status != EventStatus.EnCours)
      throw new InvalidOperationException("Seuls les événements en attente ou en cours peuvent être annulés.");

    evt.Status = EventStatus.Annule;
    evt.UpdatedAt = DateTime.UtcNow;

    await eventRepository.UpdateAsync(evt);
    await eventRepository.SaveChangesAsync();
    logger.LogInformation("Événement annulé : {Id}", id);
  }

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

  //Methodes privées

  private static void EnsureEventExists(Event? evt, Guid id)
  {
    if (evt == null)
      throw new KeyNotFoundException($"Événement {id} introuvable.");
  }

  private static void EnsureEventStatus(Event evt, EventStatus expected, string action)
  {
    if (evt.Status != expected)
      throw new InvalidOperationException($"Seuls les événements en statut '{expected}' peuvent être {action}.");
  }

  private static void ValidateEventDates(DateTime startDate, DateTime endDate, DateTime registrationDeadline)
  {
    if (startDate <= DateTime.UtcNow)
      throw new InvalidOperationException("La date de début doit être postérieure à aujourd'hui.");
    if (endDate <= startDate)
      throw new InvalidOperationException("La date de fin doit être postérieure à la date de début.");
    if (registrationDeadline > startDate)
      throw new InvalidOperationException("La date limite d'inscription doit être antérieure à la date de début.");
  }

  private static void ValidateParticipants(int min, int max)
  {
    if (min > max)
      throw new InvalidOperationException("Le nombre minimum doit être inférieur ou égal au maximum.");
  }

  private static (bool canRegister, bool isRegistered) CalculateRegistrationStatus(
    Event evt, Guid? currentUserId, int currentParticipants)
  {
    if (!currentUserId.HasValue)
      return (false, false);

    bool isRegistered = evt.SignIns.Any(s => s.UserId == currentUserId.Value && !s.IsOnWaitingList);
    bool canRegister = !isRegistered &&
                       evt.Status == EventStatus.EnAttente &&
                       evt.RegistrationDeadline >= DateTime.UtcNow &&
                       (currentParticipants < evt.MaxParticipants || evt.IsWaitingListActive);

    return (canRegister, isRegistered);
  }

  private static int? GetWaitingListPosition(Event evt, Guid? currentUserId)
  {
    if (!currentUserId.HasValue) return null;
    SignIn? signIn = evt.SignIns.FirstOrDefault(s => s.UserId == currentUserId.Value && s.IsOnWaitingList);
    return signIn?.WaitingListPosition;
  }

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
}