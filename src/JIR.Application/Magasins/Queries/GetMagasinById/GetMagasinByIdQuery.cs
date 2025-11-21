using JIR.Application.Magasins.Queries.Common;
using MediatR;

namespace JIR.Application.Magasins.Queries.GetMagasinById;

public class GetMagasinByIdQuery : IRequest<MagasinDetailDto>
{
    public Guid Id { get; set; }
}
