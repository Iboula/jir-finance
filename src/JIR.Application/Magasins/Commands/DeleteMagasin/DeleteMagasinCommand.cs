using MediatR;

namespace JIR.Application.Magasins.Commands.DeleteMagasin;

public class DeleteMagasinCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
}
