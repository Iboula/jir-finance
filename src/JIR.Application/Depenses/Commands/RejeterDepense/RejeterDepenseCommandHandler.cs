using JIR.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JIR.Application.Depenses.Commands.RejeterDepense;

public class RejeterDepenseCommandHandler : IRequestHandler<RejeterDepenseCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public RejeterDepenseCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(RejeterDepenseCommand request, CancellationToken cancellationToken)
    {
        var depense = await _context.Depenses
            .FirstOrDefaultAsync(d => d.Id == request.Id && !d.IsDeleted, cancellationToken);

        if (depense == null)
        {
            throw new Exception($"La dépense avec l'ID {request.Id} n'existe pas.");
        }

        // Vérifier le statut - On peut rejeter seulement si Brouillon ou EnAttenteValidation
        if (depense.Statut != "Brouillon" && depense.Statut != "EnAttenteValidation")
        {
            throw new Exception($"La dépense ne peut être rejetée que si elle est en statut Brouillon ou EnAttenteValidation. Statut actuel: {depense.Statut}");
        }

        depense.Statut = "Rejete";
        depense.MotifRejet = request.MotifRejet;
        depense.ValidePar = request.RejetePar; // On utilise ValidePar pour stocker qui a rejeté
        depense.DateValidation = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
