using JIR.Application.Common.Interfaces;
using JIR.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JIR.Application.MouvementsStock.Commands.DeleteMouvementStock;

public class DeleteMouvementStockCommandHandler : IRequestHandler<DeleteMouvementStockCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteMouvementStockCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteMouvementStockCommand request, CancellationToken cancellationToken)
    {
        var mouvement = await _context.MouvementsStock
            .Include(m => m.Article)
            .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

        if (mouvement == null)
        {
            throw new KeyNotFoundException($"Mouvement de stock avec l'Id {request.Id} n'existe pas.");
        }

        // Inverser l'effet du mouvement sur le stock avant suppression
        switch (mouvement.TypeMouvement)
        {
            case TypeMouvement.Entree:
                // Annuler l'entrée = retirer la quantité
                if (mouvement.Article!.QuantiteStock < mouvement.Quantite)
                {
                    throw new InvalidOperationException(
                        $"Impossible d'annuler le mouvement: stock insuffisant. Disponible: {mouvement.Article.QuantiteStock}, Requis: {mouvement.Quantite}");
                }
                mouvement.Article.QuantiteStock -= mouvement.Quantite;
                break;
            case TypeMouvement.Sortie:
                // Annuler la sortie = rajouter la quantité
                mouvement.Article!.QuantiteStock += mouvement.Quantite;
                break;
            case TypeMouvement.Ajustement:
                // Pour les ajustements, on inverse simplement l'opération
                mouvement.Article!.QuantiteStock -= mouvement.Quantite;
                break;
        }

        // Soft delete
        mouvement.IsDeleted = true;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
