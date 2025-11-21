using JIR.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JIR.Application.Sections.Commands.UpdateSection;

public record UpdateSectionCommand : IRequest<Unit>
{
    public Guid Id { get; init; }
    public string Code { get; init; } = string.Empty;
    public string Nom { get; init; } = string.Empty;
    public string? Description { get; init; }
    public decimal BudgetAnnuel { get; init; }
    public Guid? ResponsableId { get; init; }
}

public class UpdateSectionCommandHandler : IRequestHandler<UpdateSectionCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateSectionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateSectionCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Sections
            .FirstOrDefaultAsync(s => s.Id == request.Id && !s.IsDeleted, cancellationToken);

        if (entity == null)
        {
            throw new KeyNotFoundException($"Section avec l'ID {request.Id} introuvable.");
        }

        entity.Code = request.Code;
        entity.Nom = request.Nom;
        entity.Description = request.Description;
        entity.BudgetAnnuel = request.BudgetAnnuel;
        entity.ResponsableId = request.ResponsableId;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
