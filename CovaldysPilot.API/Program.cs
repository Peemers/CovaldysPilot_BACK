using CovaldysPilot.API.Extensions;
using CovaldysPilot.Application.Extensions;
using CovaldysPilot.Infrastructure.Extensions;
using Serilog;

Log.Logger = new LoggerConfiguration()
  .WriteTo.Console()
  .WriteTo.File("Logs/covaldys-.log", rollingInterval: RollingInterval.Day)
  .MinimumLevel.Information()
  .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();
builder.Services.AddOpenApi();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddRateLimiterPolicies();
builder.Services.AddJwtAuthentication(builder.Configuration);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseSerilogRequestLogging();
app.UseAuthentication();
app.UseAuthorization();

//rate-limiter désactivé en dev
if (!app.Environment.IsDevelopment())
{
  app.UseRateLimiter();
}

app.Run();