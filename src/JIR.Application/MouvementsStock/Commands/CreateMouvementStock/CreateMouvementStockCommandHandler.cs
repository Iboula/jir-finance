using JIR.Application.Common.Interfaces;
using JIR.Domain.Entities;
using JIR.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JIR.Application.MouvementsStock.Commands.CreateMouvementStock;

public class CreateMouvementStockCommandHandler : IRequestHandler<CreateMouvementStockCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateMouvementStockCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateMouvementStockCommand request, CancellationToken cancellationToken)
    {
        // Vérifier que l'article existe
        var article = await _context.ArticlesMagasin
            .FirstOrDefaultAsync(a => a.Id == request.ArticleId, cancellationToken);

        if (article == null)
        {
            throw new KeyNotFoundException($"Article avec l'Id {request.ArticleId} n'existe pas.");
        }

        // Vérifier la quantité disponible pour les sorties
        if (request.TypeMouvement == TypeMouvement.Sortie)
        {
            if (article.QuantiteStock < request.Quantite)
            {
                throw new InvalidOperationException(
                    $"Stock insuffisant. Disponible: {article.QuantiteStock}, Demandé: {request.Quantite}");
            }
        }

        // Convertir la date en UTC si nécessaire
        var dateMouvement = request.DateMouvement.Kind == DateTimeKind.Unspecified
            ? DateTime.SpecifyKind(request.DateMouvement, DateTimeKind.Utc)
            : request.DateMouvement.ToUniversalTime();

        // Créer le mouvement
        var mouvement = new MouvementStock
        {
            Id = Guid.NewGuid(),
            ArticleId = request.ArticleId,
            TypeMouvement = request.TypeMouvement,
            Quantite = request.Quantite,
            DateMouvement = dateMouvement,
            NumeroDocument = request.NumeroDocument,
            Motif = request.Motif,
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false
        };

        _context.MouvementsStock.Add(mouvement);

        // Mettre à jour le stock de l'article
        switch (request.TypeMouvement)
        {
            case TypeMouvement.Entree:
                article.QuantiteStock += request.Quantite;
                break;
            case TypeMouvement.Sortie:
                article.QuantiteStock -= request.Quantite;
                break;
            case TypeMouvement.Ajustement:
                // Pour les ajustements, la quantité peut être positive (augmentation) ou négative (diminution)
                article.QuantiteStock += request.Quantite;
                break;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return mouvement.Id;
    }
}
