using JIR.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JIR.Infrastructure.Persistence.Configurations;

public class PartenaireConfiguration : IEntityTypeConfiguration<Partenaire>
{
    public void Configure(EntityTypeBuilder<Partenaire> builder)
    {
        builder.ToTable("partenaires");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(p => p.Code)
            .HasColumnName("code")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(p => p.Nom)
            .HasColumnName("nom")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(p => p.TypePartenaire)
            .HasColumnName("type_partenaire")
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(p => p.Adresse)
            .HasColumnName("adresse")
            .HasMaxLength(500);

        builder.Property(p => p.Telephone)
            .HasColumnName("telephone")
            .HasMaxLength(20);

        builder.Property(p => p.Email)
            .HasColumnName("email")
            .HasMaxLength(100);

        builder.Property(p => p.ContactNom)
            .HasColumnName("contact_nom")
            .HasMaxLength(100);

        builder.Property(p => p.Observations)
            .HasColumnName("observations")
            .HasMaxLength(1000);

        builder.Property(p => p.IsActif)
            .HasColumnName("is_actif")
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(p => p.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(p => p.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(p => p.CreatedBy)
            .HasColumnName("created_by")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.IsDeleted)
            .HasColumnName("is_deleted")
            .IsRequired()
            .HasDefaultValue(false);

        // Indexes
        builder.HasIndex(p => p.Code)
            .IsUnique()
            .HasDatabaseName("ix_partenaires_code");

        builder.HasIndex(p => p.TypePartenaire)
            .HasDatabaseName("ix_partenaires_type");

        builder.HasIndex(p => p.IsActif)
            .HasDatabaseName("ix_partenaires_is_actif");

        builder.HasIndex(p => p.IsDeleted)
            .HasDatabaseName("ix_partenaires_is_deleted");

        // Query filter for soft delete
        builder.HasQueryFilter(p => !p.IsDeleted);
    }
}
