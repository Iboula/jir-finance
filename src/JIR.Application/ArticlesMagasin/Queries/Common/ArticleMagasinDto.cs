namespace JIR.Application.ArticlesMagasin.Queries.Common;

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
