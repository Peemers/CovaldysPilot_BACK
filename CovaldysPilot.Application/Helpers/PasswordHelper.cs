using System.Security.Cryptography;

namespace CovaldysPilot.Application.Helpers;

public static class PasswordHelper
{
  private const string Uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
  private const string Lowercase = "abcdefghijklmnopqrstuvwxyz";
  private const string Digits = "0123456789";
  private const string Special = "!@#$%^&*";

  public static string GenerateRandomPassword(int length = 12)
  {
    // On garantit au moins 1 caractère de chaque type
    // pour respecter les règles de validation du mot de passe
    List<char> passwordChars =
    [
      Uppercase[RandomNumberGenerator.GetInt32(Uppercase.Length)],
      Lowercase[RandomNumberGenerator.GetInt32(Lowercase.Length)],
      Digits[RandomNumberGenerator.GetInt32(Digits.Length)],
      Special[RandomNumberGenerator.GetInt32(Special.Length)]
    ];

    // On complète avec des caractères aléatoires du pool complet
    string allChars = Uppercase + Lowercase + Digits + Special;
    for (int i = passwordChars.Count; i < length; i++)
    {
      passwordChars.Add(allChars[RandomNumberGenerator.GetInt32(allChars.Length)]);
    }

    // On mélange pour éviter que les 4 premiers soient toujours prévisibles
    return new string(passwordChars.OrderBy(_ => RandomNumberGenerator.GetInt32(100)).ToArray());
  }
}