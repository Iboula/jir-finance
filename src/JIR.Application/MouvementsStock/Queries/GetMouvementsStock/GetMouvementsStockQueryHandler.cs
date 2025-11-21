using JIR.Application.Common.Interfaces;
using JIR.Application.MouvementsStock.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JIR.Application.MouvementsStock.Queries.GetMouvementsStock;

public class GetMouvementsStockQueryHandler : IRequestHandler<GetMouvementsStockQuery, GetMouvementsStockResponse>
{
    private readonly IApplicationDbContext _context;

    public GetMouvementsStockQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GetMouvementsStockResponse> Handle(GetMouvementsStockQuery request, CancellationToken cancellationToken)
    {
        var query = _context.MouvementsStock
            .Include(m => m.Article)
                .ThenInclude(a => a.Magasin)
            .AsQueryable();

        // Filtres
        if (request.ArticleId.HasValue)
        {
            query = query.Where(m => m.ArticleId == request.ArticleId.Value);
        }

        if (request.MagasinId.HasValue)
        {
            query = query.Where(m => m.Article.MagasinId == request.MagasinId.Value);
        }

        if (request.TypeMouvement.HasValue)
        {
            query = query.Where(m => m.TypeMouvement == request.TypeMouvement.Value);
        }

        if (request.DateDebut.HasValue)
        {
            var dateDebut = request.DateDebut.Value.Kind == DateTimeKind.Unspecified
                ? DateTime.SpecifyKind(request.DateDebut.Value, DateTimeKind.Utc)
                : request.DateDebut.Value.ToUniversalTime();
            query = query.Where(m => m.DateMouvement >= dateDebut);
        }

        if (request.DateFin.HasValue)
        {
            var dateFin = request.DateFin.Value.Kind == DateTimeKind.Unspecified
                ? DateTime.SpecifyKind(request.DateFin.Value, DateTimeKind.Utc)
                : request.DateFin.Value.ToUniversalTime();
            query = query.Where(m => m.DateMouvement <= dateFin);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var mouvements = await query
            .OrderByDescending(m => m.DateMouvement)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(m => new MouvementStockDto
            {
                Id = m.Id,
                ArticleId = m.ArticleId,
                ArticleCode = m.Article.CodeArticle,
                ArticleDesignation = m.Article.Designation,
                MagasinId = m.Article.MagasinId,
                MagasinCode = m.Article.Magasin.Code,
                MagasinNom = m.Article.Magasin.Nom,
                TypeMouvement = m.TypeMouvement,
                Quantite = m.Quantite,
                DateMouvement = m.DateMouvement,
                NumeroDocument = m.NumeroDocument,
                CreatedAt = m.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return new GetMouvementsStockResponse
        {
            Items = mouvements,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}
