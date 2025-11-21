using JIR.Application.Recettes.Queries.Common;
using MediatR;

namespace JIR.Application.Recettes.Queries.GetRecettes;

public class GetRecettesQuery : IRequest<List<RecetteDto>>
{
    public Guid? SectionId { get; set; }
    public string? Categorie { get; set; }
    public string? Source { get; set; }
    public DateTime? DateRecetteDebut { get; set; }
    public DateTime? DateRecetteFin { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 50;
}
