using MediatR;

namespace JIR.Application.Depenses.Commands.DeleteDepense;

public class DeleteDepenseCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
}
