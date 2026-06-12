namespace CovaldysPilot.Application.Interfaces.Services;

public interface IEmailService
{
  #region SendEmail
  /// <summary>
  /// Envoie un e-mail de manière asynchrone.
  /// </summary>
  /// <param name="toMail">L'adresse e-mail du destinataire.</param>
  /// <param name="toName">Le nom du destinataire.</param>
  /// <param name="subject">L'objet de l'e-mail.</param>
  /// <param name="htmlBody">Le corps du message au format HTML.</param>
  /// <returns>Une tâche représentant l'opération d'envoi d'e-mail asynchrone.</returns>
  Task SendEmail (string toMail, string toName, string subject, string htmlBody);
  #endregion
}