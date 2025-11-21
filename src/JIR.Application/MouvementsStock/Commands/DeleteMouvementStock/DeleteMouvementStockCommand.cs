using MediatR;

namespace JIR.Application.MouvementsStock.Commands.DeleteMouvementStock;

public record DeleteMouvementStockCommand(Guid Id) : IRequest<Unit>;
