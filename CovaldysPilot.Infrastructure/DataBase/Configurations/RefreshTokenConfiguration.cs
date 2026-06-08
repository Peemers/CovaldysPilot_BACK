using CovaldysPilot.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CovaldysPilot.Infrastructure.DataBase.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
  public void Configure(EntityTypeBuilder<RefreshToken> builder)
  {
    builder.HasKey(r => r.Id);

    builder.Property(r => r.Id)
      .ValueGeneratedOnAdd();

    builder.Property(r => r.Token)
      .IsRequired();

    builder.Property(r => r.ExpirationDate)
      .IsRequired();

    builder.Property(r => r.CreatedAt)
      .IsRequired()
      .ValueGeneratedOnAdd();

    builder.HasOne(r => r.User)
      .WithMany(u => u.RefreshTokens)
      .HasForeignKey(r => r.UserId)
      .OnDelete(DeleteBehavior.Cascade);
  }
}