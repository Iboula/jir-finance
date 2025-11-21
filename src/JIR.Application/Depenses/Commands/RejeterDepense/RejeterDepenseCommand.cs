using MediatR;

namespace JIR.Application.Depenses.Commands.RejeterDepense;

public class RejeterDepenseCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
    public string MotifRejet { get; set; } = string.Empty;
    public string RejetePar { get; set; } = string.Empty;
}
