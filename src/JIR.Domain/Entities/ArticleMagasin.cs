using JIR.Domain.Common;

namespace JIR.Domain.Entities;

public class ArticleMagasin : AuditableEntity
{
    public Guid MagasinId { get; set; }
    public string CodeArticle { get; set; } = string.Empty;
    public string Designation { get; set; } = string.Empty;
    public string Unite { get; set; } = string.Empty;
    public decimal QuantiteStock { get; set; }
    public decimal SeuilAlerte { get; set; }
    public decimal PrixUnitaire { get; set; }
    public string? Observations { get; set; }

    // Navigation properties
    public Magasin Magasin { get; set; } = null!;
    public ICollection<MouvementStock> Mouvements { get; set; } = new List<MouvementStock>();
}
