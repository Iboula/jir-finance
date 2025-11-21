namespace JIR.MauiApp.Models;

public class ArticleMagasinDto
{
    public Guid Id { get; set; }
    public string CodeArticle { get; set; } = string.Empty;
    public string Designation { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal QuantiteStock { get; set; }
    public decimal SeuilMinimum { get; set; }
    public decimal PrixUnitaire { get; set; }
    public Guid MagasinId { get; set; }
    public string? MagasinNom { get; set; }
}

public class CreateArticleMagasinRequest
{
    public string CodeArticle { get; set; } = string.Empty;
    public string Designation { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal SeuilMinimum { get; set; }
    public decimal PrixUnitaire { get; set; }
    public Guid MagasinId { get; set; }
}

public class UpdateArticleMagasinRequest
{
    public string Designation { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal SeuilMinimum { get; set; }
    public decimal PrixUnitaire { get; set; }
}

public class ArticlesMagasinListResult
{
    public List<ArticleMagasinDto> Articles { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
}
