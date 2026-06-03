namespace CovaldysPilot.Application.Interfaces.Services;

public interface IEmailService
{
  Task SendEmail (string toMail, string toName, string subject, string htmlBody);
}