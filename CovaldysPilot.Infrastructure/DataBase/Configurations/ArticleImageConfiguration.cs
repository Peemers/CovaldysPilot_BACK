using CovaldysPilot.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CovaldysPilot.Infrastructure.DataBase.Configurations;

public class ArticleImageConfiguration : IEntityTypeConfiguration<ArticleImage>
{
  public void Configure(EntityTypeBuilder<ArticleImage> builder)
  {
    builder.HasKey(ai => ai.Id);

    builder.Property(ai => ai.Id)
      .ValueGeneratedOnAdd();

    builder.Property(ai => ai.Url)
      .IsRequired()
      .HasMaxLength(500);

    builder.Property(ai => ai.CreatedAt)
      .IsRequired()
      .ValueGeneratedOnAdd();

    // Relation avec Article
    builder.HasOne(ai => ai.Article)
      .WithMany(a => a.Images)
      .HasForeignKey(ai => ai.ArticleId)
      .OnDelete(DeleteBehavior.Cascade);
  }
}