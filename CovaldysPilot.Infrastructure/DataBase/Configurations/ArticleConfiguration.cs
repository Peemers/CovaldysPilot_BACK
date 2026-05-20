using CovaldysPilot.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CovaldysPilot.Infrastructure.DataBase.Configurations;

public class ArticleConfiguration : IEntityTypeConfiguration<Article>
{
  public void Configure(EntityTypeBuilder<Article> builder)
  {
    builder.HasKey(a => a.Id);

    builder.Property(a => a.Id)
      .ValueGeneratedOnAdd();

    builder.Property(a => a.Title)
      .IsRequired()
      .HasMaxLength(300);

    builder.Property(a => a.Content)
      .IsRequired();

    builder.Property(a => a.Author)
      .HasMaxLength(100);

    builder.Property(a => a.CreatedAt)
      .IsRequired()
      .ValueGeneratedOnAdd();

    // Relation avec User (audit)
    builder.HasOne(a => a.User)
      .WithMany(u => u.Articles)
      .HasForeignKey(a => a.UserId)
      .OnDelete(DeleteBehavior.SetNull);
  }
}