using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

namespace CovaldysPilot.API.Extensions;

public static class RateLimiterExtensions
{
  public static IServiceCollection AddRateLimiterPolicies(this IServiceCollection services)
  {
    services.AddRateLimiter(options =>
    {
      // Politique globale : 30 requêtes par minute
      options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
        RateLimitPartition.GetFixedWindowLimiter(
          partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
          factory: _ => new FixedWindowRateLimiterOptions
          {
            PermitLimit = 30,
            Window = TimeSpan.FromMinutes(1)
          }));

      // Politique stricte pour l'authentification
      options.AddTokenBucketLimiter("auth", authOptions =>
      {
        authOptions.TokenLimit = 9;
        authOptions.TokensPerPeriod = 3;
        authOptions.ReplenishmentPeriod = TimeSpan.FromSeconds(20);
      });

      options.RejectionStatusCode = 429;
    });

    return services;
  }
}