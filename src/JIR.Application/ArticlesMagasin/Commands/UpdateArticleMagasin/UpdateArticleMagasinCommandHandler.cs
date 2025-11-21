using JIR.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JIR.Application.ArticlesMagasin.Commands.UpdateArticleMagasin;

public class UpdateArticleMagasinCommandHandler : IRequestHandler<UpdateArticleMagasinCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateArticleMagasinCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateArticleMagasinCommand request, CancellationToken cancellationToken)
    {
        var article = await _context.ArticlesMagasin
            .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

        if (article == null)
        {
            throw new KeyNotFoundException($"Article avec l'Id {request.Id} n'existe pas.");
        }

        // Vérifier l'unicité du code article dans le magasin (excluant l'article actuel)
        var codeExists = await _context.ArticlesMagasin
            .AnyAsync(a => a.MagasinId == article.MagasinId && 
                          a.CodeArticle == request.CodeArticle && 
                          a.Id != request.Id, cancellationToken);

        if (codeExists)
        {
            throw new InvalidOperationException($"Un article avec le code '{request.CodeArticle}' existe déjà dans ce magasin.");
        }

        article.CodeArticle = request.CodeArticle;
        article.Designation = request.Designation;
        article.Unite = request.Unite;
        article.SeuilAlerte = request.SeuilAlerte;
        article.PrixUnitaire = request.PrixUnitaire;
        article.Observations = request.Observations;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
