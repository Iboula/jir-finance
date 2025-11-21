using JIR.Domain.Common;

namespace JIR.Domain.Entities;

public class Recette : AuditableEntity
{
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

    // Navigation properties
    public Section Section { get; set; } = null!;
}
