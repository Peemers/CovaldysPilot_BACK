using CovaldysPilot.Application.DTOs.SiteConfiguration.Response;
using CovaldysPilot.Domain.Entities;

namespace CovaldysPilot.Application.Mappers;

public static class SiteConfigurationMapper
{
  public static SiteConfigurationResponseDto ToSiteConfigurationResponseDto(this SiteConfiguration config)
  {
    return new SiteConfigurationResponseDto
    {
      IsMaintenanceMode = config.IsMaintenanceMode,
      GlobalAlertMessage = config.GlobalAlertMessage
    };
  }
}