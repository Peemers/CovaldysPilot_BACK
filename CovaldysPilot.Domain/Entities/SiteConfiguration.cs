namespace CovaldysPilot.Domain.Entities;

public class SiteConfiguration
{
  public bool IsMaintenanceMode { get; set; } = false;
  public string? GlobalAlertMessage { get; set; }
}