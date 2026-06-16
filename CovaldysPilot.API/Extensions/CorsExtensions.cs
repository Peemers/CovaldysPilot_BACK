namespace CovaldysPilot.API.Extensions;

public static class CorsExtentions
{
  public static IServiceCollection AddCorsPolicy(this IServiceCollection services, IConfiguration configuration)
  {
    string allowedOrigins = configuration["CorsSettings:AllowedOrigins"]
                            ?? "http://localhost:4200";
    
    string[] origins = allowedOrigins.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

    services.AddCors(options =>
    {
      options.AddPolicy("CovaldysPolicy", policy =>
      {
        policy.WithOrigins(allowedOrigins)
          .AllowAnyHeader()
          .AllowAnyMethod()
          .AllowCredentials();
      });
    });

    return services;
  }
}