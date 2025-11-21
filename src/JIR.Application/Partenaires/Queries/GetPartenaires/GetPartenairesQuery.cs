using JIR.Domain.Enums;
using MediatR;

namespace JIR.Application.Partenaires.Queries.GetPartenaires;

public class GetPartenairesQuery : IRequest<GetPartenairesQueryResult>
{
    public TypePartenaire? TypePartenaire { get; set; }
    public bool? IsActif { get; set; }
    public string? Search { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 50;
}
