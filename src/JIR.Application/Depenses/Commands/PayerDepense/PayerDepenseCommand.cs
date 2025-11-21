using MediatR;

namespace JIR.Application.Depenses.Commands.PayerDepense;

public class PayerDepenseCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
    public DateTime DatePaiement { get; set; }
    public string ModePaiement { get; set; } = string.Empty;
    public string? ReferenceFacture { get; set; }
    public string? Observations { get; set; }
}
