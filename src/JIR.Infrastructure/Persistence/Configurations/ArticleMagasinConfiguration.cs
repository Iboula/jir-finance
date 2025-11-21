using JIR.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JIR.Infrastructure.Persistence.Configurations;

public class ArticleMagasinConfiguration : IEntityTypeConfiguration<ArticleMagasin>
{
    public void Configure(EntityTypeBuilder<ArticleMagasin> builder)
    {
        builder.ToTable("articles_magasin");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
            .HasColumnName("id");

        builder.Property(a => a.MagasinId)
            .HasColumnName("magasin_id")
            .IsRequired();

        builder.Property(a => a.CodeArticle)
            .HasColumnName("code_article")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(a => a.Designation)
            .HasColumnName("designation")
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(a => a.Unite)
            .HasColumnName("unite")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(a => a.QuantiteStock)
            .HasColumnName("quantite_stock")
            .HasPrecision(15, 3)
            .HasDefaultValue(0);

        builder.Property(a => a.SeuilAlerte)
            .HasColumnName("seuil_alerte")
            .HasPrecision(15, 3)
            .HasDefaultValue(0);

        builder.Property(a => a.PrixUnitaire)
            .HasColumnName("prix_unitaire")
            .HasPrecision(15, 2)
            .HasDefaultValue(0);

        builder.Property(a => a.Observations)
            .HasColumnName("observations")
            .HasMaxLength(1000);

        builder.Property(a => a.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(a => a.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(a => a.CreatedBy)
            .HasColumnName("created_by")
            .HasMaxLength(100);

        builder.Property(a => a.IsDeleted)
            .HasColumnName("is_deleted")
            .HasDefaultValue(false)
            .IsRequired();

        // Indexes
        builder.HasIndex(a => new { a.MagasinId, a.CodeArticle })
            .HasDatabaseName("ix_articles_magasin_magasin_code")
            .IsUnique();

        builder.HasIndex(a => a.IsDeleted)
            .HasDatabaseName("ix_articles_magasin_is_deleted");

        // Relationships
        builder.HasOne(a => a.Magasin)
            .WithMany(m => m.Articles)
            .HasForeignKey(a => a.MagasinId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(a => a.Mouvements)
            .WithOne(m => m.Article)
            .HasForeignKey(m => m.ArticleId)
            .OnDelete(DeleteBehavior.Restrict);

        // Query filter for soft delete
        builder.HasQueryFilter(a => !a.IsDeleted);
    }
}
