using JIR.Application.MouvementsStock.DTOs;

namespace JIR.Application.MouvementsStock.Queries.GetMouvementsStock;

public class GetMouvementsStockResponse
{
    public List<MouvementStockDto> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
}
