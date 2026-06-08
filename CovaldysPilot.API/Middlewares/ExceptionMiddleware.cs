using System.Net;
using System.Text.Json;

namespace CovaldysPilot.API.Middlewares;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
  public async Task InvokeAsync(HttpContext context)
  {
    try
    {
      await next(context);
    }
    catch (InvalidOperationException ex)
    {
      logger.LogWarning("Erreur métier : {Message}", ex.Message);
      await HandleExceptionAsync(context, HttpStatusCode.BadRequest, ex.Message);
    }
    catch (UnauthorizedAccessException ex)
    {
      logger.LogWarning("Accès non autorisé : {Message}", ex.Message);
      await HandleExceptionAsync(context, HttpStatusCode.Unauthorized, ex.Message);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Erreur non gérée : {Message}", ex.Message);
      await HandleExceptionAsync(context, HttpStatusCode.InternalServerError, "Une erreur interne est survenue.");
    }
  }

  private static async Task HandleExceptionAsync(
    HttpContext context,
    HttpStatusCode statusCode,
    string message)
  {
    context.Response.ContentType = "application/json";
    context.Response.StatusCode = (int)statusCode;

    string response = JsonSerializer.Serialize(new
    {
      statusCode = (int)statusCode,
      message
    });

    await context.Response.WriteAsync(response);
  }
}