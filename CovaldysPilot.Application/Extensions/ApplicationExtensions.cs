using CovaldysPilot.Application.Interfaces.Services;
using CovaldysPilot.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CovaldysPilot.Application.Extensions;

public static class ApplicationExtensions
{
  public static IServiceCollection AddApplicationServices(this IServiceCollection services)
  {
    services.AddScoped<IAuthService, AuthService>();
    services.AddScoped<ICategoryService, CategoryService>();
    services.AddScoped<IEventService, EventService>();
    services.AddScoped<ISignInService, SignInService>();
    services.AddScoped<IUserService, UserService>();
    services.AddScoped<IArticleService, ArticleService>();
        
    return services;
  }
}