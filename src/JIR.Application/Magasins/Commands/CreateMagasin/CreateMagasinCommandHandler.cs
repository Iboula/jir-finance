using JIR.Application.Common.Interfaces;
using JIR.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JIR.Application.Magasins.Commands.CreateMagasin;

public class CreateMagasinCommandHandler : IRequestHandler<CreateMagasinCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateMagasinCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateMagasinCommand request, CancellationToken cancellationToken)
    {
        // Vérifier l'unicité du code
        var codeExists = await _context.Magasins
            .AnyAsync(m => m.Code == request.Code, cancellationToken);

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

        var magasin = new Magasin
        {
            Id = Guid.NewGuid(),
            Code = request.Code,
            Nom = request.Nom,
            Localisation = request.Localisation,
            ResponsableId = request.ResponsableId,
            CreatedAt = DateTime.UtcNow
        };

        _context.Magasins.Add(magasin);
        await _context.SaveChangesAsync(cancellationToken);

        return magasin.Id;
    }
}
