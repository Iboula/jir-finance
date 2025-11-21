using AutoMapper;
using AutoMapper.QueryableExtensions;
using JIR.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JIR.Application.Sections.Queries.GetSections;

public record GetSectionsQuery : IRequest<List<SectionDto>>;

public class GetSectionsQueryHandler : IRequestHandler<GetSectionsQuery, List<SectionDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetSectionsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<SectionDto>> Handle(GetSectionsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Sections
            .Where(s => !s.IsDeleted)
            .OrderBy(s => s.Code)
            .ProjectTo<SectionDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}

public record SectionDto
{
    public Guid Id { get; init; }
    public string Code { get; init; } = string.Empty;
    public string Nom { get; init; } = string.Empty;
    public string? Description { get; init; }
    public decimal BudgetAnnuel { get; init; }
    public Guid? ResponsableId { get; init; }
    public string? ResponsableNom { get; init; }
    public DateTime CreatedAt { get; init; }
}
