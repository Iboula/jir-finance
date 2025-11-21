using JIR.Domain.Common;

namespace JIR.Domain.Entities;

public class Magasin : AuditableEntity
{
    public string Code { get; set; } = string.Empty;
    public string Nom { get; set; } = string.Empty;
    public string? Localisation { get; set; }
    public Guid? ResponsableId { get; set; }

    // Navigation properties
    public User? Responsable { get; set; }
    public ICollection<ArticleMagasin> Articles { get; set; } = new List<ArticleMagasin>();
}
