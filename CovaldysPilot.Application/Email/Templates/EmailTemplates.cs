namespace CovaldysPilot.Application.Email.Templates;

public static class EmailTemplates
{
  public static string RegistrationConfirmation(string firstName, string eventName, DateTime startDate, string? location)
  {
    return $"""
                <h2>Bonjour {firstName} !</h2>
                <p>Votre inscription à l'événement <strong>{eventName}</strong> est confirmée.</p>
                <p><strong>Date :</strong> {startDate:dd/MM/yyyy à HH:mm}</p>
                <p><strong>Lieu :</strong> {location ?? "À définir"}</p>
                <p>À bientôt chez Covaldys !</p>
            """;
  }

  public static string EventCancellation(string firstName, string eventName, DateTime startDate, string? reason)
  {
    return $"""
                <h2>Bonjour {firstName} !</h2>
                <p>Nous vous informons que l'événement <strong>{eventName}</strong> prévu le <strong>{startDate:dd/MM/yyyy à HH:mm}</strong> a été annulé.</p>
                {(reason != null ? $"<p><strong>Raison :</strong> {reason}</p>" : "")}
                <p>Nous nous excusons pour la gêne occasionnée.</p>
                <p>L'équipe Covaldys</p>
            """;
  }

  public static string EventReminder(string firstName, string eventName, DateTime startDate, string? location)
  {
    return $"""
                <h2>Bonjour {firstName} !</h2>
                <p>Nous vous rappelons que l'événement <strong>{eventName}</strong> aura lieu le <strong>{startDate:dd/MM/yyyy à HH:mm}</strong>.</p>
                <p><strong>Lieu :</strong> {location ?? "À définir"}</p>
                <p>Nous vous attendons nombreux !</p>
                <p>L'équipe Covaldys</p>
            """;
  }

  public static string WaitingListPromotion(string firstName, string eventName, DateTime startDate, string? location)
  {
    return $"""
                <h2>Bonjour {firstName} !</h2>
                <p>Bonne nouvelle ! Une place s'est libérée pour l'événement <strong>{eventName}</strong> prévu le <strong>{startDate:dd/MM/yyyy à HH:mm}</strong>.</p>
                <p><strong>Lieu :</strong> {location ?? "À définir"}</p>
                <p>Votre inscription est maintenant confirmée !</p>
                <p>À bientôt chez Covaldys !</p>
            """;
  }

  public static string ManualAccountCreation(string firstName, string email, string temporaryPassword)
  {
    return $"""
                <h2>Bienvenue chez Covaldys, {firstName} !</h2>
                <p>Votre compte a été créé par notre administrateur.</p>
                <p>Voici vos identifiants de connexion :</p>
                <ul>
                  <li><strong>Email :</strong> {email}</li>
                  <li><strong>Mot de passe temporaire :</strong> <strong>{temporaryPassword}</strong></li>
                </ul>
                <p>⚠️ Pour des raisons de sécurité, veuillez changer votre mot de passe dès votre première connexion.</p>
                <p>À bientôt chez Covaldys !</p>
                <p>L'équipe Covaldys</p>
            """;
  }
}