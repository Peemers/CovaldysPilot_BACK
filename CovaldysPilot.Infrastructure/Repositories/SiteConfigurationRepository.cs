using CovaldysPilot.Application.Interfaces.Repositories;
using CovaldysPilot.Domain.Entities;
using CovaldysPilot.Infrastructure.DataBase.Context;
using Microsoft.EntityFrameworkCore;

namespace CovaldysPilot.Infrastructure.Repositories;

public class SiteConfigurationRepository(CovaldysPilotDbContext context) : ISiteConfigurationRepository
{
  public async Task<SiteConfiguration> GetAsync()
  {
    SiteConfiguration? config = await context.SiteConfigurations.FirstOrDefaultAsync();
    if (config is null)
      throw new InvalidOperationException("SiteConfiguration introuvable en base de données.");
    return config;
  }

  public Task UpdateAsync(SiteConfiguration config)
  {
    context.SiteConfigurations.Update(config);
    return Task.CompletedTask;
  }

  public async Task SaveChangesAsync()
    => await context.SaveChangesAsync();
}