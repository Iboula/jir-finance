using JIR.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JIR.Application.Depenses.Commands.DeleteDepense;

public class DeleteDepenseCommandHandler : IRequestHandler<DeleteDepenseCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteDepenseCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteDepenseCommand request, CancellationToken cancellationToken)
    {
        var depense = await _context.Depenses
            .FirstOrDefaultAsync(d => d.Id == request.Id && !d.IsDeleted, cancellationToken);

        if (depense == null)
        {
            throw new Exception($"La dépense avec l'ID {request.Id} n'existe pas.");
        }

        // On ne peut supprimer qu'une dépense en statut Brouillon
        if (depense.Statut != "Brouillon")
        {
            throw new Exception($"Seules les dépenses en statut Brouillon peuvent être supprimées. Statut actuel: {depense.Statut}");
        }

        depense.IsDeleted = true;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
