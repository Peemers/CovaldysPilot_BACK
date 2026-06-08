using System.Net.Mail;
using CovaldysPilot.Application.Interfaces.Services;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace CovaldysPilot.Infrastructure.Email;
/*IOption pour récupérer la config mail de appjson sous forme d'objet très typé et l'injecter dans le service
 ça évite la manupulation et la divulgation*/
public class EmailService(
  IOptions<EmailSettings> emailSettings,
  ILogger<EmailService> logger) : IEmailService
{
  private readonly EmailSettings _settings = emailSettings.Value; //utilisation de ioption - recup de la config sous forme d'objet.

  public async Task SendEmail(string toMail, string toName, string subject, string htmlBody)
  {
    logger.LogInformation("Envoi d'un email à {ToMail} - Subject: {Subject}", toMail, subject);
    MimeMessage message = new MimeMessage();
    message.From.Add(new MailboxAddress(_settings.FromName, _settings.FromEmail)); //extraction de champs de l'objet
    message.To.Add(new MailboxAddress(toName, toMail));
    message.Subject = subject;

    message.Body = new BodyBuilder
    {
      HtmlBody = htmlBody
    }.ToMessageBody();

    using MailKit.Net.Smtp.SmtpClient client = new SmtpClient();
    
    await client.ConnectAsync(_settings.Host,  _settings.Port, SecureSocketOptions.StartTls);
    await client.AuthenticateAsync(_settings.Username, _settings.Password);
    await client.SendAsync(message);
    await client.DisconnectAsync(true);
    
    logger.LogInformation("Email envoyé {ToMail}", toMail);
  }
}