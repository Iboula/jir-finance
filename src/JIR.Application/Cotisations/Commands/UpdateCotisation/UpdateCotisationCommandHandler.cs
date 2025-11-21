using JIR.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JIR.Application.Cotisations.Commands.UpdateCotisation;

public class UpdateCotisationCommandHandler : IRequestHandler<UpdateCotisationCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateCotisationCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateCotisationCommand request, CancellationToken cancellationToken)
    {
        var cotisation = await _context.Cotisations
            .FirstOrDefaultAsync(c => c.Id == request.Id && !c.IsDeleted, cancellationToken);

        if (cotisation == null)
        {
            throw new Exception($"La cotisation avec l'ID {request.Id} n'existe pas.");
        }

        // Vérifier que la section existe
        var sectionExists = await _context.Sections
            .AnyAsync(s => s.Id == request.SectionId && !s.IsDeleted, cancellationToken);

        if (!sectionExists)
        {
            throw new Exception($"La section avec l'ID {request.SectionId} n'existe pas.");
        }

        cotisation.SectionId = request.SectionId;
        cotisation.MembreNom = request.MembreNom;
        cotisation.MembreMatricule = request.MembreMatricule;
        cotisation.Montant = request.Montant;
        cotisation.Periode = request.Periode;
        cotisation.DatePaiement = DateTime.SpecifyKind(request.DatePaiement, DateTimeKind.Utc);
        cotisation.ModePaiement = request.ModePaiement;
        cotisation.RecuNumero = request.RecuNumero;
        cotisation.Statut = request.Statut;
        cotisation.Observations = request.Observations;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
