using JIR.Application.Common.Interfaces;
using JIR.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JIR.Application.Recettes.Commands.CreateRecette;

public class CreateRecetteCommandHandler : IRequestHandler<CreateRecetteCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateRecetteCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateRecetteCommand request, CancellationToken cancellationToken)
    {
        // Vérifier que la section existe
        var sectionExists = await _context.Sections
            .AnyAsync(s => s.Id == request.SectionId && !s.IsDeleted, cancellationToken);

        if (!sectionExists)
        {
            throw new Exception($"La section avec l'ID {request.SectionId} n'existe pas.");
        }

        var recette = new Recette
        {
            Id = Guid.NewGuid(),
            SectionId = request.SectionId,
            Libelle = request.Libelle,
            Montant = request.Montant,
            DateRecette = DateTime.SpecifyKind(request.DateRecette, DateTimeKind.Utc),
            Source = request.Source,
            Categorie = request.Categorie,
            RecuNumero = request.RecuNumero,
            ModeEncaissement = request.ModeEncaissement,
            NumeroDocument = request.NumeroDocument,
            PieceJustificative = request.PieceJustificative,
            Observations = request.Observations,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "System" // TODO: Get from current user service
        };

        _context.Recettes.Add(recette);
        await _context.SaveChangesAsync(cancellationToken);

        return recette.Id;
    }
}
