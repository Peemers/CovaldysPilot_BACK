namespace CovaldysPilot.Domain.Entities;

public class RefreshToken : BaseEntity
{
  public required string Token { get; set; } = string.Empty;
  public DateTime ExpirationDate { get; set; }
  public DateTime? RevokedAt { get; set; }

  public required Guid UserId { get; set; }
  public User User { get; set; } = null!;
}