using JIR.Application.ArticlesMagasin.Queries.Common;
using JIR.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JIR.Application.ArticlesMagasin.Queries.GetArticlesMagasin;

public class GetArticlesMagasinQueryHandler : IRequestHandler<GetArticlesMagasinQuery, GetArticlesMagasinQueryResult>
{
    private readonly IApplicationDbContext _context;

    public GetArticlesMagasinQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GetArticlesMagasinQueryResult> Handle(GetArticlesMagasinQuery request, CancellationToken cancellationToken)
    {
        var query = _context.ArticlesMagasin
            .Include(a => a.Magasin)
            .Where(a => !a.IsDeleted && !a.Magasin.IsDeleted)
            .AsQueryable();

        if (request.MagasinId.HasValue)
        {
            query = query.Where(a => a.MagasinId == request.MagasinId.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var searchTerm = request.Search.ToLower();
            query = query.Where(a => a.CodeArticle.ToLower().Contains(searchTerm) || 
                                   a.Designation.ToLower().Contains(searchTerm));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var articles = await query
            .OrderBy(a => a.Magasin.Code)
            .ThenBy(a => a.CodeArticle)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(a => new ArticleMagasinDto
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
                PrixUnitaire = a.PrixUnitaire
            })
            .ToListAsync(cancellationToken);

        return new GetArticlesMagasinQueryResult
        {
            Articles = articles,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}
