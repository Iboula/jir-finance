using MediatR;

namespace JIR.Application.ArticlesMagasin.Commands.UpdateArticleMagasin;

public class UpdateArticleMagasinCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
    public string CodeArticle { get; set; } = string.Empty;
    public string Designation { get; set; } = string.Empty;
    public string Unite { get; set; } = string.Empty;
    public decimal SeuilAlerte { get; set; }
    public decimal PrixUnitaire { get; set; }
    public string? Observations { get; set; }
}
