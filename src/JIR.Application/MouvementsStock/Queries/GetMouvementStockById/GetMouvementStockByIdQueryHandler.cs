using JIR.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JIR.Application.MouvementsStock.Queries.GetMouvementStockById;

public class GetMouvementStockByIdQueryHandler : IRequestHandler<GetMouvementStockByIdQuery, MouvementStockDetailDto?>
{
    private readonly IApplicationDbContext _context;

    public GetMouvementStockByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<MouvementStockDetailDto?> Handle(GetMouvementStockByIdQuery request, CancellationToken cancellationToken)
    {
        var mouvement = await _context.MouvementsStock
            .Include(m => m.Article)
                .ThenInclude(a => a.Magasin)
            .Where(m => m.Id == request.Id)
            .Select(m => new MouvementStockDetailDto
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
                Motif = m.Motif,
                CreatedAt = m.CreatedAt,
                CreatedBy = m.CreatedBy
            })
            .FirstOrDefaultAsync(cancellationToken);

        return mouvement;
    }
}
