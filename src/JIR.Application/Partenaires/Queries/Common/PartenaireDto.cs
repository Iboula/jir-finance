using JIR.Domain.Enums;

namespace JIR.Application.Partenaires.Queries.Common;

public class PartenaireDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Nom { get; set; } = string.Empty;
    public TypePartenaire TypePartenaire { get; set; }
    public string? Telephone { get; set; }
    public string? Email { get; set; }
    public bool IsActif { get; set; }
}
