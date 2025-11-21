using JIR.Application.Common.Interfaces;
using JIR.Application.Recettes.Queries.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JIR.Application.Recettes.Queries.GetRecettes;

public class GetRecettesQueryHandler : IRequestHandler<GetRecettesQuery, List<RecetteDto>>
{
    private readonly IApplicationDbContext _context;

    public GetRecettesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<RecetteDto>> Handle(GetRecettesQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Recettes
            .Include(r => r.Section)
            .Where(r => !r.IsDeleted);

        if (request.SectionId.HasValue)
        {
            query = query.Where(r => r.SectionId == request.SectionId.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.Categorie))
        {
            query = query.Where(r => r.Categorie == request.Categorie);
        }

        if (!string.IsNullOrWhiteSpace(request.Source))
        {
            query = query.Where(r => r.Source.Contains(request.Source));
        }

        if (request.DateRecetteDebut.HasValue)
        {
            var dateDebut = DateTime.SpecifyKind(request.DateRecetteDebut.Value, DateTimeKind.Utc);
            query = query.Where(r => r.DateRecette >= dateDebut);
        }

        if (request.DateRecetteFin.HasValue)
        {
            var dateFin = DateTime.SpecifyKind(request.DateRecetteFin.Value, DateTimeKind.Utc);
            query = query.Where(r => r.DateRecette <= dateFin);
        }

        var recettes = await query
            .OrderByDescending(r => r.DateRecette)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(r => new RecetteDto
            {
                Id = r.Id,
                SectionId = r.SectionId,
                SectionCode = r.Section!.Code,
                SectionNom = r.Section.Nom,
                Libelle = r.Libelle,
                Montant = r.Montant,
                DateRecette = r.DateRecette,
                Source = r.Source,
                Categorie = r.Categorie,
                RecuNumero = r.RecuNumero,
                ModeEncaissement = r.ModeEncaissement,
                NumeroDocument = r.NumeroDocument
            })
            .ToListAsync(cancellationToken);

        return recettes;
    }
}
