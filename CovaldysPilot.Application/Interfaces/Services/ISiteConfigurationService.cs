using CovaldysPilot.Application.DTOs.SiteConfiguration.Request;
using CovaldysPilot.Application.DTOs.SiteConfiguration.Response;

namespace CovaldysPilot.Application.Interfaces.Services;

public interface ISiteConfigurationService
{
  Task<SiteConfigurationResponseDto> GetAsync();
  Task<SiteConfigurationResponseDto> UpdateMaintenanceAsync(UpdateMaintanceRequestDto dto);
  Task<SiteConfigurationResponseDto> UpdateAlertMessageAsync(UpdateAlertRequestDto dto);
}