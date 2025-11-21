using JIR.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JIR.Application.Magasins.Commands.DeleteMagasin;

public class DeleteMagasinCommandHandler : IRequestHandler<DeleteMagasinCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteMagasinCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteMagasinCommand request, CancellationToken cancellationToken)
    {
        var magasin = await _context.Magasins
            .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

        if (magasin == null)
        {
            throw new KeyNotFoundException($"Magasin avec l'Id {request.Id} n'existe pas.");
        }

        magasin.IsDeleted = true;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
