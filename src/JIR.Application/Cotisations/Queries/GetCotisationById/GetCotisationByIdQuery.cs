using MediatR;

namespace JIR.Application.Cotisations.Queries.GetCotisationById;

public class GetCotisationByIdQuery : IRequest<CotisationDetailDto>
{
    public Guid Id { get; set; }
}
