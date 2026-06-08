using CovaldysPilot.Application.Interfaces.Repositories;
using CovaldysPilot.Application.Interfaces.Services;
using CovaldysPilot.Infrastructure.DataBase.Context;
using CovaldysPilot.Infrastructure.Email;
using CovaldysPilot.Infrastructure.Repositories;
using CovaldysPilot.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CovaldysPilot.Infrastructure.Extensions;

public static class InfrastructureExtensions
{
  public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
  {
    // DbContext
    services.AddDbContext<CovaldysPilotDbContext>(options =>
      options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

    // Repositories
    services.AddScoped<IUserRepository, UserRepository>();
    services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
    services.AddScoped<ICategoryRepository, CategoryRepository>();
    services.AddScoped<IEventRepository, EventRepository>();
    services.AddScoped<ISignInRepository, SignInRepository>();
    services.AddScoped<IArticleRepository, ArticleRepository>();
    services.AddScoped<IReviewRepository, ReviewRepository>();
    services.AddScoped<ISiteConfigurationRepository, SiteConfigurationRepository>();

    // Services
    services.AddScoped<IJwtService, JwtService>();
    
    //Email
    services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
    services.AddScoped<IEmailService, EmailService>();

    return services;
  }
}