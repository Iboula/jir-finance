using JIR.Domain.Enums;
using MediatR;

namespace JIR.Application.MouvementsStock.Commands.CreateMouvementStock;

public class CreateMouvementStockCommand : IRequest<Guid>
{
    public Guid ArticleId { get; set; }
    public TypeMouvement TypeMouvement { get; set; }
    public decimal Quantite { get; set; }
    public DateTime DateMouvement { get; set; }
    public string? NumeroDocument { get; set; }
    public string? Motif { get; set; }
}
