namespace CovaldysPilot.Application.DTOs.Event.Response;

public class EventStatsResponseDto
{
  public Guid EventId { get; set; }
  public string EventName { get; set; } = string.Empty;
  public int ConfirmedParticipants { get; set; }
  public int WaitingListCount { get; set; }
  public int MaxParticipants { get; set; }
  public double FillRate { get; set; } // en %
}