namespace CovaldysPilot.Application.DTOs.SiteConfiguration.Response;

public class SiteConfigurationResponseDto
{
  public bool IsMaintenanceMode { get; init; }
  public string? GlobalAlertMessage { get; init; }
}