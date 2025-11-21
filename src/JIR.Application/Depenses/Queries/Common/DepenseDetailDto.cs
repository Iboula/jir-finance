namespace JIR.Application.Depenses.Queries.Common;

public class DepenseDetailDto
{
    public Guid Id { get; set; }
    public Guid SectionId { get; set; }
    public string SectionCode { get; set; } = string.Empty;
    public string SectionNom { get; set; } = string.Empty;
    public string Numero { get; set; } = string.Empty;
    public DateTime DateDepense { get; set; }
    public string Categorie { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Montant { get; set; }
    public string Beneficiaire { get; set; } = string.Empty;
    public string? ModePaiement { get; set; }
    public string? ReferenceFacture { get; set; }
    public string Statut { get; set; } = string.Empty;
    public DateTime? DateValidation { get; set; }
    public string? ValidePar { get; set; }
    public DateTime? DatePaiement { get; set; }
    public string? MotifRejet { get; set; }
    public string? Observations { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
