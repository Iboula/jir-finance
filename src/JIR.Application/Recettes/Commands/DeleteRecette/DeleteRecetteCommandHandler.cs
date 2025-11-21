using JIR.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JIR.Application.Recettes.Commands.DeleteRecette;

public class DeleteRecetteCommandHandler : IRequestHandler<DeleteRecetteCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteRecetteCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteRecetteCommand request, CancellationToken cancellationToken)
    {
        var recette = await _context.Recettes
            .FirstOrDefaultAsync(r => r.Id == request.Id && !r.IsDeleted, cancellationToken);

        if (recette == null)
        {
            throw new Exception($"La recette avec l'ID {request.Id} n'existe pas.");
        }

        recette.IsDeleted = true;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
