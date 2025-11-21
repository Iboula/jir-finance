using JIR.Domain.Entities;
using JIR.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JIR.Infrastructure.Persistence.Configurations;

public class MouvementStockConfiguration : IEntityTypeConfiguration<MouvementStock>
{
    public void Configure(EntityTypeBuilder<MouvementStock> builder)
    {
        builder.ToTable("mouvements_stock");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id)
            .HasColumnName("id");

        builder.Property(m => m.ArticleId)
            .HasColumnName("article_id")
            .IsRequired();

        builder.Property(m => m.TypeMouvement)
            .HasColumnName("type_mouvement")
            .HasMaxLength(50)
            .HasConversion(
                v => v.ToString(),
                v => (TypeMouvement)Enum.Parse(typeof(TypeMouvement), v))
            .IsRequired();

        builder.Property(m => m.Quantite)
            .HasColumnName("quantite")
            .HasPrecision(15, 3)
            .IsRequired();

        builder.Property(m => m.DateMouvement)
            .HasColumnName("date_mouvement")
            .IsRequired();

        builder.Property(m => m.NumeroDocument)
            .HasColumnName("numero_document")
            .HasMaxLength(100);

        builder.Property(m => m.Motif)
            .HasColumnName("motif")
            .HasMaxLength(500);

        builder.Property(m => m.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(m => m.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(m => m.CreatedBy)
            .HasColumnName("created_by")
            .HasMaxLength(100);

        builder.Property(m => m.IsDeleted)
            .HasColumnName("is_deleted")
            .HasDefaultValue(false)
            .IsRequired();

        // Indexes
        builder.HasIndex(m => m.ArticleId)
            .HasDatabaseName("ix_mouvements_stock_article_id");

        builder.HasIndex(m => m.DateMouvement)
            .HasDatabaseName("ix_mouvements_stock_date_mouvement");

        builder.HasIndex(m => m.TypeMouvement)
            .HasDatabaseName("ix_mouvements_stock_type_mouvement");

        builder.HasIndex(m => m.IsDeleted)
            .HasDatabaseName("ix_mouvements_stock_is_deleted");

        // Relationships
        builder.HasOne(m => m.Article)
            .WithMany(a => a.Mouvements)
            .HasForeignKey(m => m.ArticleId)
            .OnDelete(DeleteBehavior.Restrict);

        // Query filter for soft delete
        builder.HasQueryFilter(m => !m.IsDeleted);
    }
}
