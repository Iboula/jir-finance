using JIR.Application.Recettes.Queries.Common;
using MediatR;

namespace JIR.Application.Recettes.Queries.GetRecetteById;

public class GetRecetteByIdQuery : IRequest<RecetteDetailDto>
{
    public Guid Id { get; set; }
}
