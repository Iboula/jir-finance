using JIR.Domain.Common;

namespace JIR.Domain.Entities;

public class Cotisation : AuditableEntity
{
    public Guid SectionId { get; set; }
    public string MembreNom { get; set; } = string.Empty;
    public string MembreMatricule { get; set; } = string.Empty;
    public decimal Montant { get; set; }
    public string Periode { get; set; } = string.Empty; // Format: YYYY-MM
    public DateTime DatePaiement { get; set; }
    public string ModePaiement { get; set; } = string.Empty;
    public string? RecuNumero { get; set; }
    public string Statut { get; set; } = string.Empty;
    public string? Observations { get; set; }

    // Navigation properties
    public Section Section { get; set; } = null!;
}
