using MediatR;

namespace JIR.Application.Recettes.Commands.UpdateRecette;

public class UpdateRecetteCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
    public Guid SectionId { get; set; }
    public string Libelle { get; set; } = string.Empty;
    public decimal Montant { get; set; }
    public DateTime DateRecette { get; set; }
    public string Source { get; set; } = string.Empty;
    public string Categorie { get; set; } = string.Empty;
    public string? RecuNumero { get; set; }
    public string ModeEncaissement { get; set; } = string.Empty;
    public string? NumeroDocument { get; set; }
    public string? PieceJustificative { get; set; }
    public string? Observations { get; set; }
}
