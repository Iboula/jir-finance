using JIR.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JIR.Application.Partenaires.Commands.DeletePartenaire;

public class DeletePartenaireCommandHandler : IRequestHandler<DeletePartenaireCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeletePartenaireCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeletePartenaireCommand request, CancellationToken cancellationToken)
    {
        var partenaire = await _context.Partenaires
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (partenaire == null)
        {
            throw new KeyNotFoundException($"Partenaire avec l'Id {request.Id} n'existe pas.");
        }

        partenaire.IsDeleted = true;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
