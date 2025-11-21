using JIR.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JIR.Application.ArticlesMagasin.Commands.DeleteArticleMagasin;

public class DeleteArticleMagasinCommandHandler : IRequestHandler<DeleteArticleMagasinCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteArticleMagasinCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteArticleMagasinCommand request, CancellationToken cancellationToken)
    {
        var article = await _context.ArticlesMagasin
            .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

        if (article == null)
        {
            throw new KeyNotFoundException($"Article avec l'Id {request.Id} n'existe pas.");
        }

        article.IsDeleted = true;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
