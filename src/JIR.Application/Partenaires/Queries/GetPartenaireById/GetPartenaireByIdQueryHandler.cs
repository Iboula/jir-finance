using JIR.Application.Common.Interfaces;
using JIR.Application.Partenaires.Queries.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JIR.Application.Partenaires.Queries.GetPartenaireById;

public class GetPartenaireByIdQueryHandler : IRequestHandler<GetPartenaireByIdQuery, PartenaireDetailDto>
{
    private readonly IApplicationDbContext _context;

    public GetPartenaireByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PartenaireDetailDto> Handle(GetPartenaireByIdQuery request, CancellationToken cancellationToken)
    {
        var partenaire = await _context.Partenaires
            .Where(p => p.Id == request.Id)
            .Select(p => new PartenaireDetailDto
            {
                Id = p.Id,
                Code = p.Code,
                Nom = p.Nom,
                TypePartenaire = p.TypePartenaire,
                Adresse = p.Adresse,
                Telephone = p.Telephone,
                Email = p.Email,
                ContactNom = p.ContactNom,
                Observations = p.Observations,
                IsActif = p.IsActif,
                CreatedAt = p.CreatedAt,
                CreatedBy = p.CreatedBy,
                UpdatedAt = p.UpdatedAt
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (partenaire == null)
        {
            throw new KeyNotFoundException($"Partenaire avec l'Id {request.Id} n'existe pas.");
        }

        return partenaire;
    }
}
