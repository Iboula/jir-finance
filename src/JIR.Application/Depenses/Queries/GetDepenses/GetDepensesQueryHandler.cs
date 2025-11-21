using JIR.Application.Common.Interfaces;
using JIR.Application.Depenses.Queries.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JIR.Application.Depenses.Queries.GetDepenses;

public class GetDepensesQueryHandler : IRequestHandler<GetDepensesQuery, List<DepenseDto>>
{
    private readonly IApplicationDbContext _context;

    public GetDepensesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<DepenseDto>> Handle(GetDepensesQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Depenses
            .Include(d => d.Section)
            .Where(d => !d.IsDeleted);

        if (request.SectionId.HasValue)
        {
            query = query.Where(d => d.SectionId == request.SectionId.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.Statut))
        {
            query = query.Where(d => d.Statut == request.Statut);
        }

        if (request.DateDepenseDebut.HasValue)
        {
            var dateDebut = DateTime.SpecifyKind(request.DateDepenseDebut.Value, DateTimeKind.Utc);
            query = query.Where(d => d.DateDepense >= dateDebut);
        }

        if (request.DateDepenseFin.HasValue)
        {
            var dateFin = DateTime.SpecifyKind(request.DateDepenseFin.Value, DateTimeKind.Utc);
            query = query.Where(d => d.DateDepense <= dateFin);
        }

        var depenses = await query
            .OrderByDescending(d => d.DateDepense)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(d => new DepenseDto
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
                Observations = d.Observations
            })
            .ToListAsync(cancellationToken);

        return depenses;
    }
}
