using JIR.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JIR.Infrastructure.Persistence.Configurations;

public class DepenseConfiguration : IEntityTypeConfiguration<Depense>
{
    public void Configure(EntityTypeBuilder<Depense> builder)
    {
        builder.ToTable("depenses");

        // Primary Key
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        // Properties
        builder.Property(d => d.SectionId)
            .HasColumnName("section_id")
            .IsRequired();

        builder.Property(d => d.Numero)
            .HasColumnName("numero")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(d => d.DateDepense)
            .HasColumnName("date_depense")
            .IsRequired();

        builder.Property(d => d.Categorie)
            .HasColumnName("categorie")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(d => d.Description)
            .HasColumnName("description")
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(d => d.Montant)
            .HasColumnName("montant")
            .HasColumnType("decimal(15,2)")
            .IsRequired();

        builder.Property(d => d.Beneficiaire)
            .HasColumnName("beneficiaire")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(d => d.ModePaiement)
            .HasColumnName("mode_paiement")
            .HasMaxLength(50)
            .IsRequired(false);

        builder.Property(d => d.ReferenceFacture)
            .HasColumnName("reference_facture")
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(d => d.Statut)
            .HasColumnName("statut")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(d => d.DateValidation)
            .HasColumnName("date_validation")
            .IsRequired(false);

        builder.Property(d => d.ValidePar)
            .HasColumnName("valide_par")
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(d => d.DatePaiement)
            .HasColumnName("date_paiement")
            .IsRequired(false);

        builder.Property(d => d.MotifRejet)
            .HasColumnName("motif_rejet")
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(d => d.Observations)
            .HasColumnName("observations")
            .HasMaxLength(1000)
            .IsRequired(false);

        // Audit fields
        builder.Property(d => d.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(d => d.CreatedBy)
            .HasColumnName("created_by")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(d => d.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired(false);

        builder.Property(d => d.IsDeleted)
            .HasColumnName("is_deleted")
            .HasDefaultValue(false)
            .IsRequired();

        // Navigation properties
        builder.HasOne(d => d.Section)
            .WithMany(s => s.Depenses)
            .HasForeignKey(d => d.SectionId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(d => d.SectionId)
            .HasDatabaseName("ix_depenses_section_id");

        builder.HasIndex(d => d.Numero)
            .IsUnique()
            .HasDatabaseName("ix_depenses_numero");

        builder.HasIndex(d => d.DateDepense)
            .HasDatabaseName("ix_depenses_date_depense");

        builder.HasIndex(d => d.Statut)
            .HasDatabaseName("ix_depenses_statut");

        builder.HasIndex(d => d.IsDeleted)
            .HasDatabaseName("ix_depenses_is_deleted");

        // Query filter for soft delete
        builder.HasQueryFilter(d => !d.IsDeleted);
    }
}
