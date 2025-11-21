using MediatR;

namespace JIR.Application.ArticlesMagasin.Queries.GetArticlesMagasin;

public class GetArticlesMagasinQuery : IRequest<GetArticlesMagasinQueryResult>
{
    public Guid? MagasinId { get; set; }
    public string? Search { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 50;
}
