using JIR.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JIR.Infrastructure.Persistence.Configurations;

public class SectionConfiguration : IEntityTypeConfiguration<Section>
{
    public void Configure(EntityTypeBuilder<Section> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Code)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(e => e.Nom)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Description)
            .HasMaxLength(500);

        builder.Property(e => e.BudgetAnnuel)
            .HasPrecision(15, 2);

        builder.HasIndex(e => e.Code).IsUnique();

        builder.HasMany(e => e.Cotisations)
            .WithOne(c => c.Section)
            .HasForeignKey(c => c.SectionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.Depenses)
            .WithOne(d => d.Section)
            .HasForeignKey(d => d.SectionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.Recettes)
            .WithOne(r => r.Section)
            .HasForeignKey(r => r.SectionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
