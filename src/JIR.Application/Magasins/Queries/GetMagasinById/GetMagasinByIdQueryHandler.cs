using JIR.Application.Common.Interfaces;
using JIR.Application.Magasins.Queries.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JIR.Application.Magasins.Queries.GetMagasinById;

public class GetMagasinByIdQueryHandler : IRequestHandler<GetMagasinByIdQuery, MagasinDetailDto>
{
    private readonly IApplicationDbContext _context;

    public GetMagasinByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<MagasinDetailDto> Handle(GetMagasinByIdQuery request, CancellationToken cancellationToken)
    {
        var magasin = await _context.Magasins
            .Include(m => m.Responsable)
            .Where(m => m.Id == request.Id)
            .Select(m => new MagasinDetailDto
            {
                Id = m.Id,
                Code = m.Code,
                Nom = m.Nom,
                Localisation = m.Localisation,
                ResponsableId = m.ResponsableId,
                ResponsableNom = m.Responsable != null ? $"{m.Responsable.Prenom} {m.Responsable.Nom}" : null,
                CreatedAt = m.CreatedAt,
                CreatedBy = m.CreatedBy,
                UpdatedAt = m.UpdatedAt
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (magasin == null)
        {
            throw new KeyNotFoundException($"Magasin avec l'Id {request.Id} n'existe pas.");
        }

        return magasin;
    }
}
