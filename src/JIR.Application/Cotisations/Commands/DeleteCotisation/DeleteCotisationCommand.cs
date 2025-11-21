using MediatR;

namespace JIR.Application.Cotisations.Commands.DeleteCotisation;

public class DeleteCotisationCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
}
