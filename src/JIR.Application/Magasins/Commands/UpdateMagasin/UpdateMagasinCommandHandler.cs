using JIR.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JIR.Application.Magasins.Commands.UpdateMagasin;

public class UpdateMagasinCommandHandler : IRequestHandler<UpdateMagasinCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateMagasinCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateMagasinCommand request, CancellationToken cancellationToken)
    {
        var magasin = await _context.Magasins
            .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

        if (magasin == null)
        {
            throw new KeyNotFoundException($"Magasin avec l'Id {request.Id} n'existe pas.");
        }

        // Vérifier l'unicité du code (excluant le magasin actuel)
        var codeExists = await _context.Magasins
            .AnyAsync(m => m.Code == request.Code && m.Id != request.Id, cancellationToken);

        if (codeExists)
        {
            throw new InvalidOperationException($"Un magasin avec le code '{request.Code}' existe déjà.");
        }

        // Vérifier que le responsable existe si fourni
        if (request.ResponsableId.HasValue)
        {
            var responsableExists = await _context.Users
                .AnyAsync(u => u.Id == request.ResponsableId.Value, cancellationToken);

            if (!responsableExists)
            {
                throw new KeyNotFoundException($"Responsable avec l'Id {request.ResponsableId.Value} n'existe pas.");
            }
        }

        magasin.Code = request.Code;
        magasin.Nom = request.Nom;
        magasin.Localisation = request.Localisation;
        magasin.ResponsableId = request.ResponsableId;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
