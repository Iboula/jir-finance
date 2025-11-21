using JIR.Application.Partenaires.Queries.Common;
using MediatR;

namespace JIR.Application.Partenaires.Queries.GetPartenaireById;

public class GetPartenaireByIdQuery : IRequest<PartenaireDetailDto>
{
    public Guid Id { get; set; }
}
