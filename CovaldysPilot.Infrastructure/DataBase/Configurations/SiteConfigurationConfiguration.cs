using CovaldysPilot.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CovaldysPilot.Infrastructure.DataBase.Configurations;

public class SiteConfigurationConfiguration : IEntityTypeConfiguration<SiteConfiguration>
{
  public void Configure(EntityTypeBuilder<SiteConfiguration> builder)
  {
    builder.HasKey(sc => sc.Id);
  }
}