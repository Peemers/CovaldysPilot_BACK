using CovaldysPilot.Domain.Enums;

namespace CovaldysPilot.Domain.Entities;

public class User : BaseEntity
{
  public required string Pseudo { get; set; } = string.Empty;
  public required string Email { get; set; } = string.Empty;
  public required string PasswordHash { get; set; } = string.Empty;
  public required Role Role { get; set; } = Role.Membre;
  public Genre Gender { get; set; }
  public required DateTime Birthday { get; set; }
  public bool IsMemberShipUpToDate { get; set; }
  public DateTime? LastPayementDate { get; set; }

  public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
  public ICollection<SignIn> SignIns { get; set; } = new List<SignIn>();
  public ICollection<Review> Reviews { get; set; } = new List<Review>();
  public ICollection<Article> Articles { get; set; } = new List<Article>();
}