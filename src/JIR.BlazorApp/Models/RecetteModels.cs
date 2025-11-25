using System.ComponentModel.DataAnnotations;

namespace JIR.BlazorApp.Models;

public class RecetteDto
{
    public Guid Id { get; set; }
    public Guid SectionId { get; set; }
    public string SectionCode { get; set; } = string.Empty;
    public string SectionNom { get; set; } = string.Empty;
    public DateTime DateRecette { get; set; }
    public string Source { get; set; } = string.Empty;
    public string Categorie { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Montant { get; set; }
    public string? NomDonateur { get; set; }
    public string? Reference { get; set; }
    public string? Observations { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateRecetteDto
{
    [Required(ErrorMessage = "La section est obligatoire")]
    public Guid SectionId { get; set; }

    [Required(ErrorMessage = "La date est obligatoire")]
    public DateTime DateRecette { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "La source est obligatoire")]
    [MaxLength(50, ErrorMessage = "Maximum 50 caractères")]
    public string Source { get; set; } = string.Empty;

    [Required(ErrorMessage = "La catégorie est obligatoire")]
    [MaxLength(100, ErrorMessage = "Maximum 100 caractères")]
    public string Categorie { get; set; } = string.Empty;

    [Required(ErrorMessage = "La description est obligatoire")]
    [MaxLength(500, ErrorMessage = "Maximum 500 caractères")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Le montant est obligatoire")]
    [Range(0.01, 999999999.99, ErrorMessage = "Le montant doit être positif")]
    public decimal Montant { get; set; }

    [MaxLength(200, ErrorMessage = "Maximum 200 caractères")]
    public string? NomDonateur { get; set; }

    [MaxLength(100, ErrorMessage = "Maximum 100 caractères")]
    public string? Reference { get; set; }

    [MaxLength(1000, ErrorMessage = "Maximum 1000 caractères")]
    public string? Observations { get; set; }
}

public class UpdateRecetteDto
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "La section est obligatoire")]
    public Guid SectionId { get; set; }

    [Required(ErrorMessage = "La date est obligatoire")]
    public DateTime DateRecette { get; set; }

    [Required(ErrorMessage = "La source est obligatoire")]
    [MaxLength(50, ErrorMessage = "Maximum 50 caractères")]
    public string Source { get; set; } = string.Empty;

    [Required(ErrorMessage = "La catégorie est obligatoire")]
    [MaxLength(100, ErrorMessage = "Maximum 100 caractères")]
    public string Categorie { get; set; } = string.Empty;

    [Required(ErrorMessage = "La description est obligatoire")]
    [MaxLength(500, ErrorMessage = "Maximum 500 caractères")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Le montant est obligatoire")]
    [Range(0.01, 999999999.99, ErrorMessage = "Le montant doit être positif")]
    public decimal Montant { get; set; }

    [MaxLength(200, ErrorMessage = "Maximum 200 caractères")]
    public string? NomDonateur { get; set; }

    [MaxLength(100, ErrorMessage = "Maximum 100 caractères")]
    public string? Reference { get; set; }

    [MaxLength(1000, ErrorMessage = "Maximum 1000 caractères")]
    public string? Observations { get; set; }
}

public static class SourceRecette
{
    public const string Subvention = "Subvention";
    public const string Don = "Don";
    public const string VenteArticle = "Vente Article";
    public const string Cotisation = "Cotisation";
    public const string Autre = "Autre";

    public static List<string> GetAll()
    {
        return new List<string>
        {
            Subvention,
            Don,
            VenteArticle,
            Cotisation,
            Autre
        };
    }

    public static string GetBadgeClass(string source)
    {
        return source switch
        {
            Subvention => "badge bg-primary",
            Don => "badge bg-success",
            VenteArticle => "badge bg-info",
            Cotisation => "badge bg-warning",
            Autre => "badge bg-secondary",
            _ => "badge bg-secondary"
        };
    }
}
