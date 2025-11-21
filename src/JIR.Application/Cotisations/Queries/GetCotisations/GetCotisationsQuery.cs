using MediatR;

namespace JIR.Application.Cotisations.Queries.GetCotisations;

public class GetCotisationsQuery : IRequest<List<CotisationDto>>
{
    public Guid? SectionId { get; set; }
    public string? Periode { get; set; }
    public DateTime? DatePaiementDebut { get; set; }
    public DateTime? DatePaiementFin { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 50;
}
