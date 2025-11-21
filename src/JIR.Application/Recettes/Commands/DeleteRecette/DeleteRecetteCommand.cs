using MediatR;

namespace JIR.Application.Recettes.Commands.DeleteRecette;

public class DeleteRecetteCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
}
