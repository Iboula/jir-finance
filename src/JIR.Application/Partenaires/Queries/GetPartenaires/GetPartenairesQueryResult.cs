using JIR.Application.Partenaires.Queries.Common;

namespace JIR.Application.Partenaires.Queries.GetPartenaires;

public class GetPartenairesQueryResult
{
    public List<PartenaireDto> Partenaires { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
}
