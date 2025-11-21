using JIR.Domain.Common;
using JIR.Domain.Enums;

namespace JIR.Domain.Entities;

public class Partenaire : AuditableEntity
{
    public string Code { get; set; } = string.Empty;
    public string Nom { get; set; } = string.Empty;
    public TypePartenaire TypePartenaire { get; set; }
    public string? Adresse { get; set; }
    public string? Telephone { get; set; }
    public string? Email { get; set; }
    public string? ContactNom { get; set; }
    public string? Observations { get; set; }
    public bool IsActif { get; set; } = true;
}
