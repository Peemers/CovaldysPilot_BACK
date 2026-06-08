using CovaldysPilot.API.Extensions;
using CovaldysPilot.API.Middlewares;
using CovaldysPilot.API.Scalar;
using CovaldysPilot.Application.Extensions;
using CovaldysPilot.Infrastructure.DataBase.Context;
using CovaldysPilot.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Serilog;

Log.Logger = new LoggerConfiguration()
  .WriteTo.Console()
  .WriteTo.File("Logs/covaldys-.log", rollingInterval: RollingInterval.Day)
  .MinimumLevel.Information()
  .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();
builder.Services.AddOpenApi(options =>
{
  options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
});
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddRateLimiterPolicies();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddCorsPolicy(builder.Configuration);
builder.Services.AddControllers()
  .AddJsonOptions(options =>
  {
    options.JsonSerializerOptions.Converters.Add(
      new System.Text.Json.Serialization.JsonStringEnumConverter()
    );
  });


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>(); // tout premier middleware
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(option =>
    {
      option.Title = "Covaldys Pilot API";
      option.Theme = ScalarTheme.Moon;
      option.AddHttpAuthentication("Bearer", bearer =>
      {
        bearer.Token = "votre-token-jwt";
      });
    });
}

app.UseHttpsRedirection();
app.UseCors("CovaldysPolicy");
app.UseSerilogRequestLogging();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

//rate-limiter désactivé en dev
if (!app.Environment.IsDevelopment())
{
  app.UseRateLimiter();
}

using (var scope = app.Services.CreateScope()) //verification de migration et applique en auto la derniere.
{
  var db = scope.ServiceProvider.GetRequiredService<CovaldysPilotDbContext>();
  db.Database.Migrate();
}

app.Run();