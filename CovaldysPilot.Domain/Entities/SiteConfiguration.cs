namespace CovaldysPilot.Domain.Entities;

public class SiteConfiguration
{
  public int Id { get; set; } = 1;
  public bool IsMaintenanceMode { get; set; } = false;
  public string? GlobalAlertMessage { get; set; }
}