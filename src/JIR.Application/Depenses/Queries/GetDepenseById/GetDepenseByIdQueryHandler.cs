using JIR.Application.Common.Interfaces;
using JIR.Application.Depenses.Queries.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JIR.Application.Depenses.Queries.GetDepenseById;

public class GetDepenseByIdQueryHandler : IRequestHandler<GetDepenseByIdQuery, DepenseDetailDto>
{
    private readonly IApplicationDbContext _context;

    public GetDepenseByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DepenseDetailDto> Handle(GetDepenseByIdQuery request, CancellationToken cancellationToken)
    {
        var depense = await _context.Depenses
            .Include(d => d.Section)
            .Where(d => d.Id == request.Id && !d.IsDeleted)
            .Select(d => new DepenseDetailDto
            {
                Id = d.Id,
                SectionId = d.SectionId,
                SectionCode = d.Section!.Code,
                SectionNom = d.Section.Nom,
                Numero = d.Numero,
                DateDepense = d.DateDepense,
                Categorie = d.Categorie,
                Description = d.Description,
                Montant = d.Montant,
                Beneficiaire = d.Beneficiaire,
                ModePaiement = d.ModePaiement,
                ReferenceFacture = d.ReferenceFacture,
                Statut = d.Statut,
                DateValidation = d.DateValidation,
                ValidePar = d.ValidePar,
                DatePaiement = d.DatePaiement,
                MotifRejet = d.MotifRejet,
                Observations = d.Observations,
                CreatedAt = d.CreatedAt,
                UpdatedAt = d.UpdatedAt
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (depense == null)
        {
            throw new Exception($"La dépense avec l'ID {request.Id} n'existe pas.");
        }

        return depense;
    }
}
