using JIR.Application.Common.Interfaces;
using JIR.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JIR.Application.ArticlesMagasin.Commands.CreateArticleMagasin;

public class CreateArticleMagasinCommandHandler : IRequestHandler<CreateArticleMagasinCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateArticleMagasinCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateArticleMagasinCommand request, CancellationToken cancellationToken)
    {
        // Vérifier que le magasin existe
        var magasinExists = await _context.Magasins
            .AnyAsync(m => m.Id == request.MagasinId, cancellationToken);

        if (!magasinExists)
        {
            throw new KeyNotFoundException($"Magasin avec l'Id {request.MagasinId} n'existe pas.");
        }

        // Vérifier l'unicité du code article dans le magasin
        var codeExists = await _context.ArticlesMagasin
            .AnyAsync(a => a.MagasinId == request.MagasinId && a.CodeArticle == request.CodeArticle, cancellationToken);

        if (codeExists)
        {
            throw new InvalidOperationException($"Un article avec le code '{request.CodeArticle}' existe déjà dans ce magasin.");
        }

        var article = new ArticleMagasin
        {
            Id = Guid.NewGuid(),
            MagasinId = request.MagasinId,
            CodeArticle = request.CodeArticle,
            Designation = request.Designation,
            Unite = request.Unite,
            QuantiteStock = 0, // Toujours 0 à la création
            SeuilAlerte = request.SeuilAlerte,
            PrixUnitaire = request.PrixUnitaire,
            Observations = request.Observations,
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false
        };

        _context.ArticlesMagasin.Add(article);
        await _context.SaveChangesAsync(cancellationToken);

        return article.Id;
    }
}
