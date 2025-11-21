using JIR.Domain.Common;

namespace JIR.Domain.Entities;

public class Depense : AuditableEntity
{
    public Guid SectionId { get; set; }
    public string Numero { get; set; } = string.Empty; // Numéro unique de la dépense
    public DateTime DateDepense { get; set; }
    public string Categorie { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Montant { get; set; }
    public string Beneficiaire { get; set; } = string.Empty;
    public string? ModePaiement { get; set; }
    public string? ReferenceFacture { get; set; }
    public string Statut { get; set; } = "Brouillon"; // Brouillon, EnAttenteValidation, Validé, Rejeté, Payé
    public DateTime? DateValidation { get; set; }
    public string? ValidePar { get; set; } // Nom de la personne qui a validé
    public DateTime? DatePaiement { get; set; }
    public string? MotifRejet { get; set; }
    public string? Observations { get; set; }

    // Navigation properties
    public Section Section { get; set; } = null!;
}
