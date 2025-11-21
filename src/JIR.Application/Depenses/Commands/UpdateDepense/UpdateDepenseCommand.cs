using MediatR;

namespace JIR.Application.Depenses.Commands.UpdateDepense;

public class UpdateDepenseCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
    public Guid SectionId { get; set; }
    public string Numero { get; set; } = string.Empty;
    public DateTime DateDepense { get; set; }
    public string Categorie { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Montant { get; set; }
    public string Beneficiaire { get; set; } = string.Empty;
    public string? ModePaiement { get; set; }
    public string? ReferenceFacture { get; set; }
    public string? Observations { get; set; }
}
