using AutoMapper;
using AutoMapper.QueryableExtensions;
using JIR.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JIR.Application.Cotisations.Queries.GetCotisationById;

public class GetCotisationByIdQueryHandler : IRequestHandler<GetCotisationByIdQuery, CotisationDetailDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCotisationByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CotisationDetailDto> Handle(GetCotisationByIdQuery request, CancellationToken cancellationToken)
    {
        var cotisation = await _context.Cotisations
            .Include(c => c.Section)
            .Where(c => c.Id == request.Id && !c.IsDeleted)
            .ProjectTo<CotisationDetailDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (cotisation == null)
        {
            throw new Exception($"La cotisation avec l'ID {request.Id} n'existe pas.");
        }

        return cotisation;
    }
}
