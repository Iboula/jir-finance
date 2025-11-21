using AutoMapper;
using AutoMapper.QueryableExtensions;
using JIR.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JIR.Application.Sections.Queries.GetSectionById;

public record GetSectionByIdQuery(Guid Id) : IRequest<SectionDetailDto?>;

public class GetSectionByIdQueryHandler : IRequestHandler<GetSectionByIdQuery, SectionDetailDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetSectionByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<SectionDetailDto?> Handle(GetSectionByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.Sections
            .Where(s => s.Id == request.Id && !s.IsDeleted)
            .ProjectTo<SectionDetailDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
    }
}

public record SectionDetailDto
{
    public Guid Id { get; init; }
    public string Code { get; init; } = string.Empty;
    public string Nom { get; init; } = string.Empty;
    public string? Description { get; init; }
    public decimal BudgetAnnuel { get; init; }
    public Guid? ResponsableId { get; init; }
    public string? ResponsableNom { get; init; }
    public int NombreMembres { get; init; }
    public int NombreCotisations { get; init; }
    public int NombreDepenses { get; init; }
    public DateTime CreatedAt { get; init; }
}
