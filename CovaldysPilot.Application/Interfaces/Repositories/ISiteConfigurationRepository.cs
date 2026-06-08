using CovaldysPilot.Domain.Entities;

namespace CovaldysPilot.Application.Interfaces.Repositories;

public interface ISiteConfigurationRepository
{
  Task<SiteConfiguration> GetAsync();
  Task UpdateAsync(SiteConfiguration config);
  Task SaveChangesAsync();
}