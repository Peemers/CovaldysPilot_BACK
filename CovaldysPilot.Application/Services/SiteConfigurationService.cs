using CovaldysPilot.Application.DTOs.SiteConfiguration.Request;
using CovaldysPilot.Application.DTOs.SiteConfiguration.Response;
using CovaldysPilot.Application.Interfaces.Repositories;
using CovaldysPilot.Application.Interfaces.Services;
using CovaldysPilot.Application.Mappers;
using CovaldysPilot.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace CovaldysPilot.Application.Services;

public class SiteConfigurationService(
    ISiteConfigurationRepository siteConfigurationRepository,
    ILogger<SiteConfigurationService> logger) : ISiteConfigurationService
{
    public async Task<SiteConfigurationResponseDto> GetAsync()
    {
        logger.LogInformation("Récupération de la configuration du site");
        SiteConfiguration config = await siteConfigurationRepository.GetAsync();
        return config.ToSiteConfigurationResponseDto();
    }

    public async Task<SiteConfigurationResponseDto> UpdateMaintenanceAsync(UpdateMaintanceRequestDto dto)
    {
        logger.LogInformation("Mise à jour du mode maintenance : {IsMaintenanceMode}", dto.IsMaintenanceMode);
        SiteConfiguration config = await siteConfigurationRepository.GetAsync();
        config.IsMaintenanceMode = dto.IsMaintenanceMode;
        await siteConfigurationRepository.UpdateAsync(config);
        await siteConfigurationRepository.SaveChangesAsync();
        logger.LogInformation("Mode maintenance mis à jour : {IsMaintenanceMode}", dto.IsMaintenanceMode);
        return config.ToSiteConfigurationResponseDto();
    }

    public async Task<SiteConfigurationResponseDto> UpdateAlertMessageAsync(UpdateAlertRequestDto dto)
    {
        logger.LogInformation("Mise à jour du message d'alerte : {Message}", dto.GlobalAlertMessage);
        SiteConfiguration config = await siteConfigurationRepository.GetAsync();
        config.GlobalAlertMessage = dto.GlobalAlertMessage;
        await siteConfigurationRepository.UpdateAsync(config);
        await siteConfigurationRepository.SaveChangesAsync();
        logger.LogInformation("Message d'alerte mis à jour");
        return config.ToSiteConfigurationResponseDto();
    }
}