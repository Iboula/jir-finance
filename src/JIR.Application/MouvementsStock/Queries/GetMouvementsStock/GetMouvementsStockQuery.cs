using JIR.Domain.Enums;
using MediatR;

namespace JIR.Application.MouvementsStock.Queries.GetMouvementsStock;

public record GetMouvementsStockQuery : IRequest<GetMouvementsStockResponse>
{
    public Guid? ArticleId { get; init; }
    public Guid? MagasinId { get; init; }
    public TypeMouvement? TypeMouvement { get; init; }
    public DateTime? DateDebut { get; init; }
    public DateTime? DateFin { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 50;
}
