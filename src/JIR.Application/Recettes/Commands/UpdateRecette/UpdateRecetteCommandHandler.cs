using JIR.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JIR.Application.Recettes.Commands.UpdateRecette;

public class UpdateRecetteCommandHandler : IRequestHandler<UpdateRecetteCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateRecetteCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateRecetteCommand request, CancellationToken cancellationToken)
    {
        var recette = await _context.Recettes
            .FirstOrDefaultAsync(r => r.Id == request.Id && !r.IsDeleted, cancellationToken);

        if (recette == null)
        {
            throw new Exception($"La recette avec l'ID {request.Id} n'existe pas.");
        }

        // Vérifier que la section existe
        var sectionExists = await _context.Sections
            .AnyAsync(s => s.Id == request.SectionId && !s.IsDeleted, cancellationToken);

        if (!sectionExists)
        {
            throw new Exception($"La section avec l'ID {request.SectionId} n'existe pas.");
        }

        recette.SectionId = request.SectionId;
        recette.Libelle = request.Libelle;
        recette.Montant = request.Montant;
        recette.DateRecette = DateTime.SpecifyKind(request.DateRecette, DateTimeKind.Utc);
        recette.Source = request.Source;
        recette.Categorie = request.Categorie;
        recette.RecuNumero = request.RecuNumero;
        recette.ModeEncaissement = request.ModeEncaissement;
        recette.NumeroDocument = request.NumeroDocument;
        recette.PieceJustificative = request.PieceJustificative;
        recette.Observations = request.Observations;
        recette.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
