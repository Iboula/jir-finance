using JIR.Domain.Enums;

namespace JIR.Application.Partenaires.Queries.Common;

public class PartenaireDetailDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Nom { get; set; } = string.Empty;
    public TypePartenaire TypePartenaire { get; set; }
    public string? Adresse { get; set; }
    public string? Telephone { get; set; }
    public string? Email { get; set; }
    public string? ContactNom { get; set; }
    public string? Observations { get; set; }
    public bool IsActif { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
