using JIR.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JIR.Application.Depenses.Commands.UpdateDepense;

public class UpdateDepenseCommandHandler : IRequestHandler<UpdateDepenseCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateDepenseCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateDepenseCommand request, CancellationToken cancellationToken)
    {
        var depense = await _context.Depenses
            .FirstOrDefaultAsync(d => d.Id == request.Id && !d.IsDeleted, cancellationToken);

        if (depense == null)
        {
            throw new Exception($"La dépense avec l'ID {request.Id} n'existe pas.");
        }

        // On ne peut modifier qu'une dépense en statut Brouillon
        if (depense.Statut != "Brouillon")
        {
            throw new Exception($"Seules les dépenses en statut Brouillon peuvent être modifiées. Statut actuel: {depense.Statut}");
        }

        // Vérifier que la section existe
        var sectionExists = await _context.Sections
            .AnyAsync(s => s.Id == request.SectionId && !s.IsDeleted, cancellationToken);

        if (!sectionExists)
        {
            throw new Exception($"La section avec l'ID {request.SectionId} n'existe pas.");
        }

        // Vérifier que le numéro n'existe pas déjà (sauf pour cette dépense)
        var numeroExists = await _context.Depenses
            .AnyAsync(d => d.Numero == request.Numero && d.Id != request.Id && !d.IsDeleted, cancellationToken);

        if (numeroExists)
        {
            throw new Exception($"Une autre dépense avec le numéro {request.Numero} existe déjà.");
        }

        depense.SectionId = request.SectionId;
        depense.Numero = request.Numero;
        depense.DateDepense = DateTime.SpecifyKind(request.DateDepense, DateTimeKind.Utc);
        depense.Categorie = request.Categorie;
        depense.Description = request.Description;
        depense.Montant = request.Montant;
        depense.Beneficiaire = request.Beneficiaire;
        depense.ModePaiement = request.ModePaiement;
        depense.ReferenceFacture = request.ReferenceFacture;
        depense.Observations = request.Observations;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
