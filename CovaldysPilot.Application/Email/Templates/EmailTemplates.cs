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
}