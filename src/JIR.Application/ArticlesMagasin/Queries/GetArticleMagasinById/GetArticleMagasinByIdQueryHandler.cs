using JIR.Application.ArticlesMagasin.Queries.Common;
using JIR.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JIR.Application.ArticlesMagasin.Queries.GetArticleMagasinById;

public class GetArticleMagasinByIdQueryHandler : IRequestHandler<GetArticleMagasinByIdQuery, ArticleMagasinDetailDto>
{
    private readonly IApplicationDbContext _context;

    public GetArticleMagasinByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ArticleMagasinDetailDto> Handle(GetArticleMagasinByIdQuery request, CancellationToken cancellationToken)
    {
        var article = await _context.ArticlesMagasin
            .Include(a => a.Magasin)
            .Where(a => a.Id == request.Id)
            .Select(a => new ArticleMagasinDetailDto
            {
                Id = a.Id,
                MagasinId = a.MagasinId,
                MagasinCode = a.Magasin.Code,
                MagasinNom = a.Magasin.Nom,
                CodeArticle = a.CodeArticle,
                Designation = a.Designation,
                Unite = a.Unite,
                QuantiteStock = a.QuantiteStock,
                SeuilAlerte = a.SeuilAlerte,
                PrixUnitaire = a.PrixUnitaire,
                Observations = a.Observations,
                CreatedAt = a.CreatedAt,
                CreatedBy = a.CreatedBy,
                UpdatedAt = a.UpdatedAt
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (article == null)
        {
            throw new KeyNotFoundException($"Article avec l'Id {request.Id} n'existe pas.");
        }

        return article;
    }
}
