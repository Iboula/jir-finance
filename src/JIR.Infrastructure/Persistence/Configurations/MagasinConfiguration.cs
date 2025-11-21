using JIR.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JIR.Infrastructure.Persistence.Configurations;

public class MagasinConfiguration : IEntityTypeConfiguration<Magasin>
{
    public void Configure(EntityTypeBuilder<Magasin> builder)
    {
        builder.ToTable("magasins");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id)
            .HasColumnName("id");

        builder.Property(m => m.Code)
            .HasColumnName("code")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(m => m.Nom)
            .HasColumnName("nom")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(m => m.Localisation)
            .HasColumnName("localisation")
            .HasMaxLength(500);

        builder.Property(m => m.ResponsableId)
            .HasColumnName("responsable_id");

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
        builder.HasIndex(m => m.Code)
            .HasDatabaseName("ix_magasins_code")
            .IsUnique();

        builder.HasIndex(m => m.IsDeleted)
            .HasDatabaseName("ix_magasins_is_deleted");

        // Relationships
        builder.HasOne(m => m.Responsable)
            .WithMany()
            .HasForeignKey(m => m.ResponsableId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(m => m.Articles)
            .WithOne(a => a.Magasin)
            .HasForeignKey(a => a.MagasinId)
            .OnDelete(DeleteBehavior.Restrict);

        // Query filter for soft delete
        builder.HasQueryFilter(m => !m.IsDeleted);
    }
}
