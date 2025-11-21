using JIR.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JIR.Application.Depenses.Commands.PayerDepense;

public class PayerDepenseCommandHandler : IRequestHandler<PayerDepenseCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public PayerDepenseCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(PayerDepenseCommand request, CancellationToken cancellationToken)
    {
        var depense = await _context.Depenses
            .FirstOrDefaultAsync(d => d.Id == request.Id && !d.IsDeleted, cancellationToken);

        if (depense == null)
        {
            throw new Exception($"La dépense avec l'ID {request.Id} n'existe pas.");
        }

        // Vérifier le statut - On peut payer seulement si Validé
        if (depense.Statut != "Valide")
        {
            throw new Exception($"La dépense ne peut être payée que si elle est validée. Statut actuel: {depense.Statut}");
        }

        depense.Statut = "Paye";
        depense.DatePaiement = DateTime.SpecifyKind(request.DatePaiement, DateTimeKind.Utc);
        depense.ModePaiement = request.ModePaiement;
        
        if (!string.IsNullOrEmpty(request.ReferenceFacture))
        {
            depense.ReferenceFacture = request.ReferenceFacture;
        }

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
