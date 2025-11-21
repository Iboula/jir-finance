namespace JIR.BlazorApp.Models;

public class ArticleMagasinDto
{
    public Guid Id { get; set; }
    public Guid MagasinId { get; set; }
    public string MagasinCode { get; set; } = string.Empty;
    public string MagasinNom { get; set; } = string.Empty;
    public string CodeArticle { get; set; } = string.Empty;
    public string Designation { get; set; } = string.Empty;
    public string Unite { get; set; } = string.Empty;
    public decimal QuantiteStock { get; set; }
    public decimal SeuilAlerte { get; set; }
    public decimal PrixUnitaire { get; set; }
}

public class CreateArticleMagasinRequest
{
    public Guid MagasinId { get; set; }
    public string CodeArticle { get; set; } = string.Empty;
    public string Designation { get; set; } = string.Empty;
    public string Unite { get; set; } = string.Empty;
    public decimal SeuilAlerte { get; set; }
    public decimal PrixUnitaire { get; set; }
    public string? Observations { get; set; }
}

public class UpdateArticleMagasinRequest
{
    public string CodeArticle { get; set; } = string.Empty;
    public string Designation { get; set; } = string.Empty;
    public string Unite { get; set; } = string.Empty;
    public decimal SeuilAlerte { get; set; }
    public decimal PrixUnitaire { get; set; }
    public string? Observations { get; set; }
}

public class ArticlesMagasinListResult
{
    public List<ArticleMagasinDto> Articles { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
}
