using MediatR;

namespace JIR.Application.MouvementsStock.Queries.GetMouvementStockById;

public record GetMouvementStockByIdQuery(Guid Id) : IRequest<MouvementStockDetailDto?>;
