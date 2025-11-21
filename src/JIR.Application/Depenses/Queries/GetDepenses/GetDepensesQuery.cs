using JIR.Application.Depenses.Queries.Common;
using MediatR;

namespace JIR.Application.Depenses.Queries.GetDepenses;

public class GetDepensesQuery : IRequest<List<DepenseDto>>
{
    public Guid? SectionId { get; set; }
    public string? Statut { get; set; }
    public DateTime? DateDepenseDebut { get; set; }
    public DateTime? DateDepenseFin { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 50;
}
