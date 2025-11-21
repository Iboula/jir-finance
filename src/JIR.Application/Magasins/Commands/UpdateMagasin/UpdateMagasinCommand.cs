using MediatR;

namespace JIR.Application.Magasins.Commands.UpdateMagasin;

public class UpdateMagasinCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Nom { get; set; } = string.Empty;
    public string? Localisation { get; set; }
    public Guid? ResponsableId { get; set; }
}
