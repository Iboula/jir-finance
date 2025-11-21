using JIR.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JIR.Application.Cotisations.Commands.DeleteCotisation;

public class DeleteCotisationCommandHandler : IRequestHandler<DeleteCotisationCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteCotisationCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteCotisationCommand request, CancellationToken cancellationToken)
    {
        var cotisation = await _context.Cotisations
            .FirstOrDefaultAsync(c => c.Id == request.Id && !c.IsDeleted, cancellationToken);

        if (cotisation == null)
        {
            throw new Exception($"La cotisation avec l'ID {request.Id} n'existe pas.");
        }

        // Soft delete
        cotisation.IsDeleted = true;
        cotisation.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
