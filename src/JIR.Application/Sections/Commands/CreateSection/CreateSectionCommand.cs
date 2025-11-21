using JIR.Application.Common.Interfaces;
using JIR.Domain.Entities;
using MediatR;

namespace JIR.Application.Sections.Commands.CreateSection;

public record CreateSectionCommand : IRequest<Guid>
{
    public string Code { get; init; } = string.Empty;
    public string Nom { get; init; } = string.Empty;
    public string? Description { get; init; }
    public decimal BudgetAnnuel { get; init; }
    public Guid? ResponsableId { get; init; }
}

public class CreateSectionCommandHandler : IRequestHandler<CreateSectionCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateSectionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateSectionCommand request, CancellationToken cancellationToken)
    {
        var entity = new Section
        {
            Id = Guid.NewGuid(),
            Code = request.Code,
            Nom = request.Nom,
            Description = request.Description,
            BudgetAnnuel = request.BudgetAnnuel,
            ResponsableId = request.ResponsableId,
            CreatedAt = DateTime.UtcNow
        };

        _context.Sections.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
