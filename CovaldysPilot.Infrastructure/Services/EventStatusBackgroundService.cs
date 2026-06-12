using CovaldysPilot.Application.Interfaces.Repositories;
using CovaldysPilot.Domain.Entities;
using CovaldysPilot.Domain.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CovaldysPilot.Infrastructure.Services;

public class EventStatusBackgroundService(
  IServiceScopeFactory scopeFactory,
  ILogger<EventStatusBackgroundService> logger) : BackgroundService
{
  // vérifie toutes les 30 secondes
  private readonly TimeSpan _interval = TimeSpan.FromSeconds(30);

  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    logger.LogInformation("EventStatusBackgroundService démarré");

    while (!stoppingToken.IsCancellationRequested)
    {
      try
      {
        await CheckAndUpdateEventStatusesAsync();
      }
      catch (Exception ex)
      {
        logger.LogError(ex, "Erreur dans EventStatusBackgroundService");
      }

      await Task.Delay(_interval, stoppingToken);
    }

    logger.LogInformation("EventStatusBackgroundService arrêté");
  }

  private async Task CheckAndUpdateEventStatusesAsync()
  {
    // IHostedService est Singleton → on crée un scope pour accéder aux repos Scoped
    using IServiceScope scope = scopeFactory.CreateScope();
    IEventRepository eventRepository = scope.ServiceProvider.GetRequiredService<IEventRepository>();

    DateTime now = DateTime.Now;

    // EnAttente → EnCours si DateDebut ≤ now et pas Annulé
    IEnumerable<Event> eventsToStart = await eventRepository.GetByStatusAsync(EventStatus.EnAttente);
    foreach (Event evt in eventsToStart)
    {
      if (evt.StartDate <= now)
      {
        evt.Status = EventStatus.EnCours;
        evt.UpdatedAt = now;
        await eventRepository.UpdateAsync(evt);
        logger.LogInformation("Événement {Id} passé en EnCours automatiquement", evt.Id);
      }
    }

    // EnCours → Terminé si DateFin ≤ now
    IEnumerable<Event> eventsToClose = await eventRepository.GetByStatusAsync(EventStatus.EnCours);
    foreach (Event evt in eventsToClose)
    {
      if (evt.EndDate <= now)
      {
        evt.Status = EventStatus.Termine;
        evt.UpdatedAt = now;
        await eventRepository.UpdateAsync(evt);
        logger.LogInformation("Événement {Id} passé en Terminé automatiquement", evt.Id);
      }
    }

    await eventRepository.SaveChangesAsync();
  }
}