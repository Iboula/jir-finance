using MediatR;

namespace JIR.Application.Partenaires.Commands.DeletePartenaire;

public class DeletePartenaireCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
}
