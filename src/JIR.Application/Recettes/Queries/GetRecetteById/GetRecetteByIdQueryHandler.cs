using JIR.Application.Common.Interfaces;
using JIR.Application.Recettes.Queries.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JIR.Application.Recettes.Queries.GetRecetteById;

public class GetRecetteByIdQueryHandler : IRequestHandler<GetRecetteByIdQuery, RecetteDetailDto>
{
    private readonly IApplicationDbContext _context;

    public GetRecetteByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RecetteDetailDto> Handle(GetRecetteByIdQuery request, CancellationToken cancellationToken)
    {
        var recette = await _context.Recettes
            .Include(r => r.Section)
            .Where(r => r.Id == request.Id && !r.IsDeleted)
            .Select(r => new RecetteDetailDto
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
                NumeroDocument = r.NumeroDocument,
                PieceJustificative = r.PieceJustificative,
                Observations = r.Observations,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (recette == null)
        {
            throw new Exception($"La recette avec l'ID {request.Id} n'existe pas.");
        }

        return recette;
    }
}
