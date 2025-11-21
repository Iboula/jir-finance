using JIR.Domain.Common;
using JIR.Domain.Enums;

namespace JIR.Domain.Entities;

public class MouvementStock : AuditableEntity
{
    public Guid ArticleId { get; set; }
    public TypeMouvement TypeMouvement { get; set; }
    public decimal Quantite { get; set; }
    public DateTime DateMouvement { get; set; }
    public string? NumeroDocument { get; set; }
    public string? Motif { get; set; }

    // Navigation properties
    public ArticleMagasin Article { get; set; } = null!;
}
