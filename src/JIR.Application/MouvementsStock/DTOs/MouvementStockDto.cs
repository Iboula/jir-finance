using JIR.Domain.Enums;

namespace JIR.Application.MouvementsStock.DTOs;

public class MouvementStockDto
{
    public Guid Id { get; set; }
    public Guid ArticleId { get; set; }
    public string ArticleCode { get; set; } = string.Empty;
    public string ArticleDesignation { get; set; } = string.Empty;
    public Guid MagasinId { get; set; }
    public string MagasinCode { get; set; } = string.Empty;
    public string MagasinNom { get; set; } = string.Empty;
    public TypeMouvement TypeMouvement { get; set; }
    public decimal Quantite { get; set; }
    public DateTime DateMouvement { get; set; }
    public string? NumeroDocument { get; set; }
    public DateTime CreatedAt { get; set; }
}
