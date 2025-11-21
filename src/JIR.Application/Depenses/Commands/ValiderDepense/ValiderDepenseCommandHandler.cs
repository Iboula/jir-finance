using JIR.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JIR.Application.Depenses.Commands.ValiderDepense;

public class ValiderDepenseCommandHandler : IRequestHandler<ValiderDepenseCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public ValiderDepenseCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(ValiderDepenseCommand request, CancellationToken cancellationToken)
    {
        var depense = await _context.Depenses
            .FirstOrDefaultAsync(d => d.Id == request.Id && !d.IsDeleted, cancellationToken);

        if (depense == null)
        {
            throw new Exception($"La dépense avec l'ID {request.Id} n'existe pas.");
        }

        // Vérifier le statut - On peut valider seulement si Brouillon ou EnAttenteValidation
        if (depense.Statut != "Brouillon" && depense.Statut != "EnAttenteValidation")
        {
            throw new Exception($"La dépense ne peut être validée que si elle est en statut Brouillon ou EnAttenteValidation. Statut actuel: {depense.Statut}");
        }

        depense.Statut = "Valide";
        depense.DateValidation = DateTime.UtcNow;
        depense.ValidePar = request.ValidePar;
        
        if (!string.IsNullOrEmpty(request.Observations))
        {
            depense.Observations = string.IsNullOrEmpty(depense.Observations) 
                ? request.Observations 
                : $"{depense.Observations}\n{request.Observations}";
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
