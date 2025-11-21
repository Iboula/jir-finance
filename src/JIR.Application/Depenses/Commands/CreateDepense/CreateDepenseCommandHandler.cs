using JIR.Application.Common.Interfaces;
using JIR.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JIR.Application.Depenses.Commands.CreateDepense;

public class CreateDepenseCommandHandler : IRequestHandler<CreateDepenseCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateDepenseCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateDepenseCommand request, CancellationToken cancellationToken)
    {
        // Vérifier que la section existe
        var sectionExists = await _context.Sections
            .AnyAsync(s => s.Id == request.SectionId && !s.IsDeleted, cancellationToken);

        if (!sectionExists)
        {
            throw new Exception($"La section avec l'ID {request.SectionId} n'existe pas.");
        }

        // Vérifier que le numéro n'existe pas déjà
        var numeroExists = await _context.Depenses
            .AnyAsync(d => d.Numero == request.Numero && !d.IsDeleted, cancellationToken);

        if (numeroExists)
        {
            throw new Exception($"Une dépense avec le numéro {request.Numero} existe déjà.");
        }

        var depense = new Depense
        {
            Id = Guid.NewGuid(),
            SectionId = request.SectionId,
            Numero = request.Numero,
            DateDepense = DateTime.SpecifyKind(request.DateDepense, DateTimeKind.Utc),
            Categorie = request.Categorie,
            Description = request.Description,
            Montant = request.Montant,
            Beneficiaire = request.Beneficiaire,
            ModePaiement = request.ModePaiement,
            ReferenceFacture = request.ReferenceFacture,
            Statut = "Brouillon", // Statut initial
            Observations = request.Observations
        };

        _context.Depenses.Add(depense);
        await _context.SaveChangesAsync(cancellationToken);

        return depense.Id;
    }
}
