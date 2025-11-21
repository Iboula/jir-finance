using MediatR;

namespace JIR.Application.Magasins.Commands.CreateMagasin;

public class CreateMagasinCommand : IRequest<Guid>
{
    public string Code { get; set; } = string.Empty;
    public string Nom { get; set; } = string.Empty;
    public string? Localisation { get; set; }
    public Guid? ResponsableId { get; set; }
}
