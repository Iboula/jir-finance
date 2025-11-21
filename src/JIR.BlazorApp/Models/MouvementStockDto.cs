namespace JIR.BlazorApp.Models;

public enum TypeMouvement
{
    Entree,
    Sortie,
    Ajustement
}

public class MouvementStockDto
{
    public Guid Id { get; set; }
    public Guid ArticleId { get; set; }
    public string? ArticleDesignation { get; set; }
    public Guid MagasinId { get; set; }
    public string? MagasinNom { get; set; }
    public TypeMouvement TypeMouvement { get; set; }
    public decimal Quantite { get; set; }
    public DateTime DateMouvement { get; set; }
    public string? NumeroDocument { get; set; }
    public string? Motif { get; set; }
}

public class CreateMouvementStockRequest
{
    public Guid ArticleId { get; set; }
    public TypeMouvement TypeMouvement { get; set; }
    public decimal Quantite { get; set; }
    public DateTime DateMouvement { get; set; }
    public string? NumeroDocument { get; set; }
    public string? Motif { get; set; }
}

public class MouvementsStockListResult
{
    public List<MouvementStockDto> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
}
