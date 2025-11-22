namespace JIR.BlazorApp.Models;

public class CotisationDto
{
    public Guid Id { get; set; }
    public Guid SectionId { get; set; }
    public string SectionNom { get; set; } = string.Empty;
    public string MembreNom { get; set; } = string.Empty;
    public string MembreMatricule { get; set; } = string.Empty;
    public decimal Montant { get; set; }
    public string Periode { get; set; } = string.Empty;
    public DateTime DatePaiement { get; set; }
    public string ModePaiement { get; set; } = string.Empty;
    public string? RecuNumero { get; set; }
    public string Statut { get; set; } = string.Empty;
    public string? Observations { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CotisationDetailDto
{
    public Guid Id { get; set; }
    public Guid SectionId { get; set; }
    public string SectionNom { get; set; } = string.Empty;
    public string SectionCode { get; set; } = string.Empty;
    public string MembreNom { get; set; } = string.Empty;
    public string MembreMatricule { get; set; } = string.Empty;
    public decimal Montant { get; set; }
    public string Periode { get; set; } = string.Empty;
    public DateTime DatePaiement { get; set; }
    public string ModePaiement { get; set; } = string.Empty;
    public string? RecuNumero { get; set; }
    public string Statut { get; set; } = string.Empty;
    public string? Observations { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
}

public class CreateCotisationDto
{
    public Guid SectionId { get; set; }
    public string MembreNom { get; set; } = string.Empty;
    public string MembreMatricule { get; set; } = string.Empty;
    public decimal Montant { get; set; }
    public string Periode { get; set; } = string.Empty;
    public DateTime DatePaiement { get; set; } = DateTime.Now;
    public string ModePaiement { get; set; } = "Espèces";
    public string? RecuNumero { get; set; }
    public string Statut { get; set; } = "Validé";
    public string? Observations { get; set; }
}

public class UpdateCotisationDto
{
    public Guid Id { get; set; }
    public Guid SectionId { get; set; }
    public string MembreNom { get; set; } = string.Empty;
    public string MembreMatricule { get; set; } = string.Empty;
    public decimal Montant { get; set; }
    public string Periode { get; set; } = string.Empty;
    public DateTime DatePaiement { get; set; }
    public string ModePaiement { get; set; } = string.Empty;
    public string? RecuNumero { get; set; }
    public string Statut { get; set; } = string.Empty;
    public string? Observations { get; set; }
}
