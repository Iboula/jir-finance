using JIR.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JIR.Infrastructure.Persistence.Configurations;

public class RecetteConfiguration : IEntityTypeConfiguration<Recette>
{
    public void Configure(EntityTypeBuilder<Recette> builder)
    {
        builder.ToTable("recettes");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(r => r.SectionId)
            .HasColumnName("section_id")
            .IsRequired();

        builder.Property(r => r.Libelle)
            .HasColumnName("libelle")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(r => r.Montant)
            .HasColumnName("montant")
            .HasColumnType("decimal(15,2)")
            .IsRequired();

        builder.Property(r => r.DateRecette)
            .HasColumnName("date_recette")
            .IsRequired();

        builder.Property(r => r.Source)
            .HasColumnName("source")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(r => r.Categorie)
            .HasColumnName("categorie")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(r => r.RecuNumero)
            .HasColumnName("recu_numero")
            .HasMaxLength(50);

        builder.Property(r => r.ModeEncaissement)
            .HasColumnName("mode_encaissement")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(r => r.NumeroDocument)
            .HasColumnName("numero_document")
            .HasMaxLength(100);

        builder.Property(r => r.PieceJustificative)
            .HasColumnName("piece_justificative")
            .HasMaxLength(500);

        builder.Property(r => r.Observations)
            .HasColumnName("observations")
            .HasMaxLength(1000);

        builder.Property(r => r.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(r => r.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(r => r.CreatedBy)
            .HasColumnName("created_by")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(r => r.IsDeleted)
            .HasColumnName("is_deleted")
            .IsRequired()
            .HasDefaultValue(false);

        // Foreign key relationship
        builder.HasOne(r => r.Section)
            .WithMany()
            .HasForeignKey(r => r.SectionId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(r => r.SectionId)
            .HasDatabaseName("ix_recettes_section_id");

        builder.HasIndex(r => r.DateRecette)
            .HasDatabaseName("ix_recettes_date_recette");

        builder.HasIndex(r => r.Categorie)
            .HasDatabaseName("ix_recettes_categorie");

        builder.HasIndex(r => r.Source)
            .HasDatabaseName("ix_recettes_source");

        builder.HasIndex(r => r.IsDeleted)
            .HasDatabaseName("ix_recettes_is_deleted");

        builder.HasIndex(r => new { r.SectionId, r.DateRecette })
            .HasDatabaseName("ix_recettes_section_date");

        // Query filter for soft delete
        builder.HasQueryFilter(r => !r.IsDeleted);
    }
}
