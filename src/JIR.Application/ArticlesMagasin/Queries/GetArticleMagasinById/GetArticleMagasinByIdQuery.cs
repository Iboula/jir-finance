using JIR.Application.ArticlesMagasin.Queries.Common;
using MediatR;

namespace JIR.Application.ArticlesMagasin.Queries.GetArticleMagasinById;

public class GetArticleMagasinByIdQuery : IRequest<ArticleMagasinDetailDto>
{
    public Guid Id { get; set; }
}
