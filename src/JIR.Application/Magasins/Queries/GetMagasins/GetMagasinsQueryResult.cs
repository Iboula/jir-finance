using JIR.Application.Magasins.Queries.Common;

namespace JIR.Application.Magasins.Queries.GetMagasins;

public class GetMagasinsQueryResult
{
    public List<MagasinDto> Magasins { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
}
