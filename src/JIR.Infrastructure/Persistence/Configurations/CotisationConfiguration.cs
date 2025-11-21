using JIR.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JIR.Infrastructure.Persistence.Configurations;

public class CotisationConfiguration : IEntityTypeConfiguration<Cotisation>
{
    public void Configure(EntityTypeBuilder<Cotisation> builder)
    {
        builder.ToTable("cotisations");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(c => c.SectionId)
            .HasColumnName("section_id")
            .IsRequired();

        builder.Property(c => c.MembreNom)
            .HasColumnName("membre_nom")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(c => c.MembreMatricule)
            .HasColumnName("membre_matricule")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.Montant)
            .HasColumnName("montant")
            .HasColumnType("decimal(15,2)")
            .IsRequired();

        builder.Property(c => c.Periode)
            .HasColumnName("periode")
            .HasMaxLength(7)
            .IsRequired();

        builder.Property(c => c.DatePaiement)
            .HasColumnName("date_paiement")
            .IsRequired();

        builder.Property(c => c.ModePaiement)
            .HasColumnName("mode_paiement")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.RecuNumero)
            .HasColumnName("recu_numero")
            .HasMaxLength(50);

        builder.Property(c => c.Statut)
            .HasColumnName("statut")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.Observations)
            .HasColumnName("observations")
            .HasMaxLength(500);

        builder.Property(c => c.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(c => c.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(c => c.CreatedBy)
            .HasColumnName("created_by")
            .HasMaxLength(255);

        builder.Property(c => c.IsDeleted)
            .HasColumnName("is_deleted")
            .HasDefaultValue(false);

        // Relations
        builder.HasOne(c => c.Section)
            .WithMany(s => s.Cotisations)
            .HasForeignKey(c => c.SectionId)
            .OnDelete(DeleteBehavior.Restrict);

        // Index
        builder.HasIndex(c => c.SectionId)
            .HasDatabaseName("ix_cotisations_section_id");

        builder.HasIndex(c => c.Periode)
            .HasDatabaseName("ix_cotisations_periode");

        builder.HasIndex(c => c.DatePaiement)
            .HasDatabaseName("ix_cotisations_date_paiement");

        builder.HasIndex(c => c.IsDeleted)
            .HasDatabaseName("ix_cotisations_is_deleted");
    }
}
