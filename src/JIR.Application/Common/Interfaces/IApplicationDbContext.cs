using JIR.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace JIR.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<Section> Sections { get; }
    DbSet<Cotisation> Cotisations { get; }
    DbSet<Depense> Depenses { get; }
    DbSet<Recette> Recettes { get; }
    DbSet<Partenaire> Partenaires { get; }
    DbSet<Magasin> Magasins { get; }
    DbSet<ArticleMagasin> ArticlesMagasin { get; }
    DbSet<MouvementStock> MouvementsStock { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
