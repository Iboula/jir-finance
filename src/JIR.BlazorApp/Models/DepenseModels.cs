using System.ComponentModel.DataAnnotations;

namespace JIR.BlazorApp.Models;

public class DepenseDto
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
}

public class CreateDepenseDto
{
    [Required(ErrorMessage = "La section est requise")]
    public Guid SectionId { get; set; }

    [Required(ErrorMessage = "Le numéro est requis")]
    [MaxLength(50)]
    public string Numero { get; set; } = string.Empty;

    [Required(ErrorMessage = "La date de dépense est requise")]
    public DateTime DateDepense { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "La catégorie est requise")]
    [MaxLength(100)]
    public string Categorie { get; set; } = string.Empty;

    [Required(ErrorMessage = "La description est requise")]
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Le montant est requis")]
    [Range(0.01, 999999999.99, ErrorMessage = "Le montant doit être supérieur à 0")]
    public decimal Montant { get; set; }

    [Required(ErrorMessage = "Le bénéficiaire est requis")]
    [MaxLength(200)]
    public string Beneficiaire { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? ModePaiement { get; set; }

    [MaxLength(100)]
    public string? ReferenceFacture { get; set; }

    [MaxLength(1000)]
    public string? Observations { get; set; }
}

public class UpdateDepenseDto
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "La section est requise")]
    public Guid SectionId { get; set; }

    [Required(ErrorMessage = "Le numéro est requis")]
    [MaxLength(50)]
    public string Numero { get; set; } = string.Empty;

    [Required(ErrorMessage = "La date de dépense est requise")]
    public DateTime DateDepense { get; set; }

    [Required(ErrorMessage = "La catégorie est requise")]
    [MaxLength(100)]
    public string Categorie { get; set; } = string.Empty;

    [Required(ErrorMessage = "La description est requise")]
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Le montant est requis")]
    [Range(0.01, 999999999.99, ErrorMessage = "Le montant doit être supérieur à 0")]
    public decimal Montant { get; set; }

    [Required(ErrorMessage = "Le bénéficiaire est requis")]
    [MaxLength(200)]
    public string Beneficiaire { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? ModePaiement { get; set; }

    [MaxLength(100)]
    public string? ReferenceFacture { get; set; }

    [MaxLength(1000)]
    public string? Observations { get; set; }
}

public class ValiderDepenseDto
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Le nom du validateur est requis")]
    [MaxLength(200)]
    public string ValidePar { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Observations { get; set; }
}

public class RejeterDepenseDto
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Le motif de rejet est requis")]
    [MaxLength(500)]
    public string MotifRejet { get; set; } = string.Empty;
}

public class PayerDepenseDto
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Le mode de paiement est requis")]
    [MaxLength(50)]
    public string ModePaiement { get; set; } = string.Empty;

    [Required(ErrorMessage = "La date de paiement est requise")]
    public DateTime DatePaiement { get; set; } = DateTime.Now;
}

public static class StatutDepense
{
    public const string Brouillon = "Brouillon";
    public const string EnAttenteValidation = "EnAttenteValidation";
    public const string Valide = "Valide";
    public const string Rejete = "Rejete";
    public const string Paye = "Paye";

    public static List<string> GetAll() => new()
    {
        Brouillon,
        EnAttenteValidation,
        Valide,
        Rejete,
        Paye
    };

    public static string GetBadgeClass(string statut) => statut switch
    {
        Brouillon => "badge bg-secondary",
        EnAttenteValidation => "badge bg-warning",
        Valide => "badge bg-success",
        Rejete => "badge bg-danger",
        Paye => "badge bg-primary",
        _ => "badge bg-secondary"
    };

    public static string GetLabel(string statut) => statut switch
    {
        Brouillon => "Brouillon",
        EnAttenteValidation => "En attente",
        Valide => "Validé",
        Rejete => "Rejeté",
        Paye => "Payé",
        _ => statut
    };
}
