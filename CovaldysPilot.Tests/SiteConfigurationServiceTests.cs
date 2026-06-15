using System.Threading.Tasks;
using CovaldysPilot.Application.DTOs.SiteConfiguration.Request;
using CovaldysPilot.Application.DTOs.SiteConfiguration.Response;
using CovaldysPilot.Application.Interfaces.Repositories;
using CovaldysPilot.Application.Services;
using CovaldysPilot.Domain.Entities;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace CovaldysPilot.Tests;

public class SiteConfigurationServiceTests
{
    private readonly ISiteConfigurationRepository _siteConfigurationRepository;
    private readonly ILogger<SiteConfigurationService> _logger;
    private readonly SiteConfigurationService _siteConfigurationService;

    public SiteConfigurationServiceTests()
    {
        _siteConfigurationRepository = Substitute.For<ISiteConfigurationRepository>();
        _logger = Substitute.For<ILogger<SiteConfigurationService>>();

        _siteConfigurationService = new SiteConfigurationService(
            _siteConfigurationRepository,
            _logger
        );
    }

    #region GetAsync Tests

    [Fact]
    public async Task GetAsync_ReturnsMappedSiteConfiguration()
    {
        // Arrange
        var config = new SiteConfiguration
        {
            Id = 1,
            IsMaintenanceMode = true,
            GlobalAlertMessage = "Important Alert Message"
        };

        _siteConfigurationRepository.GetAsync().Returns(config);

        // Act
        var result = await _siteConfigurationService.GetAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(config.IsMaintenanceMode, result.IsMaintenanceMode);
        Assert.Equal(config.GlobalAlertMessage, result.GlobalAlertMessage);

        await _siteConfigurationRepository.Received(1).GetAsync();
    }

    #endregion

    #region UpdateMaintenanceAsync Tests

    [Fact]
    public async Task UpdateMaintenanceAsync_UpdatesMaintenanceModeAndSaves()
    {
        // Arrange
        var request = new UpdateMaintanceRequestDto
        {
            IsMaintenanceMode = true
        };

        var config = new SiteConfiguration
        {
            Id = 1,
            IsMaintenanceMode = false,
            GlobalAlertMessage = "Alert"
        };

        _siteConfigurationRepository.GetAsync().Returns(config);

        // Act
        var result = await _siteConfigurationService.UpdateMaintenanceAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsMaintenanceMode);
        Assert.Equal(config.GlobalAlertMessage, result.GlobalAlertMessage);

        await _siteConfigurationRepository.Received(1).GetAsync();
        await _siteConfigurationRepository.Received(1).UpdateAsync(Arg.Is<SiteConfiguration>(c => c.IsMaintenanceMode == true));
        await _siteConfigurationRepository.Received(1).SaveChangesAsync();
    }

    #endregion

    #region UpdateAlertMessageAsync Tests

    [Fact]
    public async Task UpdateAlertMessageAsync_UpdatesAlertMessageAndSaves()
    {
        // Arrange
        var request = new UpdateAlertRequestDto
        {
            GlobalAlertMessage = "New global alert message"
        };

        var config = new SiteConfiguration
        {
            Id = 1,
            IsMaintenanceMode = false,
            GlobalAlertMessage = "Old Alert"
        };

        _siteConfigurationRepository.GetAsync().Returns(config);

        // Act
        var result = await _siteConfigurationService.UpdateAlertMessageAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(request.GlobalAlertMessage, result.GlobalAlertMessage);
        Assert.False(result.IsMaintenanceMode);

        await _siteConfigurationRepository.Received(1).GetAsync();
        await _siteConfigurationRepository.Received(1).UpdateAsync(Arg.Is<SiteConfiguration>(c => c.GlobalAlertMessage == request.GlobalAlertMessage));
        await _siteConfigurationRepository.Received(1).SaveChangesAsync();
    }

    #endregion
}
