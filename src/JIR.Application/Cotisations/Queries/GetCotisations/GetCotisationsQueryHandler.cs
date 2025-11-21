using AutoMapper;
using AutoMapper.QueryableExtensions;
using JIR.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JIR.Application.Cotisations.Queries.GetCotisations;

public class GetCotisationsQueryHandler : IRequestHandler<GetCotisationsQuery, List<CotisationDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCotisationsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<CotisationDto>> Handle(GetCotisationsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Cotisations
            .Include(c => c.Section)
            .Where(c => !c.IsDeleted)
            .AsQueryable();

        // Filtrer par section si spécifié
        if (request.SectionId.HasValue)
        {
            query = query.Where(c => c.SectionId == request.SectionId.Value);
        }

        // Filtrer par période si spécifié
        if (!string.IsNullOrEmpty(request.Periode))
        {
            query = query.Where(c => c.Periode == request.Periode);
        }

        // Filtrer par date de paiement
        if (request.DatePaiementDebut.HasValue)
        {
            query = query.Where(c => c.DatePaiement >= request.DatePaiementDebut.Value);
        }

        if (request.DatePaiementFin.HasValue)
        {
            query = query.Where(c => c.DatePaiement <= request.DatePaiementFin.Value);
        }

        var cotisations = await query
            .OrderByDescending(c => c.DatePaiement)
            .ThenBy(c => c.MembreNom)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectTo<CotisationDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return cotisations;
    }
}
