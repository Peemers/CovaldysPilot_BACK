using System.Threading.RateLimiting;
using CovaldysPilot.Infrastructure.DataBase.Context;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Serilog;

Log.Logger = new LoggerConfiguration()
  .WriteTo.Console()
  .WriteTo.File("Logs/covaldys-.log", rollingInterval: RollingInterval.Day)
  .MinimumLevel.Information()
  .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();
builder.Services.AddOpenApi();
builder.Services.AddDbContext<CovaldysPilotDbContext>(options =>
  options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddRateLimiter(options =>
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

  // Politique stricte pour l'authentification : 5 tentatives par 5 minutes
  options.AddTokenBucketLimiter("auth", authOptions =>
  {
    authOptions.TokenLimit = 9;
    authOptions.TokensPerPeriod = 3;
    authOptions.ReplenishmentPeriod = TimeSpan.FromSeconds(20);
  });

  options.RejectionStatusCode = 429;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseSerilogRequestLogging();

//rate-limiter désactivé en dev
if (!app.Environment.IsDevelopment())
{
  app.UseRateLimiter();
}

app.Run();