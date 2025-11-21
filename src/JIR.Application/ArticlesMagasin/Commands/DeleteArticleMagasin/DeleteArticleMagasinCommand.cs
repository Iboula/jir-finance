using MediatR;

namespace JIR.Application.ArticlesMagasin.Commands.DeleteArticleMagasin;

public class DeleteArticleMagasinCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
}
