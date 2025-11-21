using MediatR;

namespace JIR.Application.Depenses.Commands.ValiderDepense;

public class ValiderDepenseCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
    public string ValidePar { get; set; } = string.Empty; // Nom de la personne qui valide
    public string? Observations { get; set; }
}
