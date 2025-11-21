using JIR.Application.Common.Interfaces;
using JIR.Application.Magasins.Queries.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JIR.Application.Magasins.Queries.GetMagasins;

public class GetMagasinsQueryHandler : IRequestHandler<GetMagasinsQuery, GetMagasinsQueryResult>
{
    private readonly IApplicationDbContext _context;

    public GetMagasinsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GetMagasinsQueryResult> Handle(GetMagasinsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Magasins
            .Include(m => m.Responsable)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var searchTerm = request.Search.ToLower();
            query = query.Where(m => m.Code.ToLower().Contains(searchTerm) || 
                                   m.Nom.ToLower().Contains(searchTerm));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var magasins = await query
            .OrderBy(m => m.Code)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(m => new MagasinDto
            {
                Id = m.Id,
                Code = m.Code,
                Nom = m.Nom,
                Localisation = m.Localisation,
                ResponsableNom = m.Responsable != null ? $"{m.Responsable.Prenom} {m.Responsable.Nom}" : null
            })
            .ToListAsync(cancellationToken);

        return new GetMagasinsQueryResult
        {
            Magasins = magasins,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}
