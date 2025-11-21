using JIR.Application.Common.Interfaces;
using JIR.Application.Partenaires.Queries.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JIR.Application.Partenaires.Queries.GetPartenaires;

public class GetPartenairesQueryHandler : IRequestHandler<GetPartenairesQuery, GetPartenairesQueryResult>
{
    private readonly IApplicationDbContext _context;

    public GetPartenairesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GetPartenairesQueryResult> Handle(GetPartenairesQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Partenaires.AsQueryable();

        if (request.TypePartenaire.HasValue)
        {
            query = query.Where(p => p.TypePartenaire == request.TypePartenaire.Value);
        }

        if (request.IsActif.HasValue)
        {
            query = query.Where(p => p.IsActif == request.IsActif.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var searchTerm = request.Search.ToLower();
            query = query.Where(p => p.Code.ToLower().Contains(searchTerm) || 
                                   p.Nom.ToLower().Contains(searchTerm));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var partenaires = await query
            .OrderBy(p => p.Code)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(p => new PartenaireDto
            {
                Id = p.Id,
                Code = p.Code,
                Nom = p.Nom,
                TypePartenaire = p.TypePartenaire,
                Telephone = p.Telephone,
                Email = p.Email,
                IsActif = p.IsActif
            })
            .ToListAsync(cancellationToken);

        return new GetPartenairesQueryResult
        {
            Partenaires = partenaires,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}
