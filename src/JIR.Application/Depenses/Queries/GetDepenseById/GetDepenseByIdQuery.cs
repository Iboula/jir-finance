using JIR.Application.Depenses.Queries.Common;
using MediatR;

namespace JIR.Application.Depenses.Queries.GetDepenseById;

public class GetDepenseByIdQuery : IRequest<DepenseDetailDto>
{
    public Guid Id { get; set; }
}
