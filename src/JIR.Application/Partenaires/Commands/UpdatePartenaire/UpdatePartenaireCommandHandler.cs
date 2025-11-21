using JIR.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JIR.Application.Partenaires.Commands.UpdatePartenaire;

public class UpdatePartenaireCommandHandler : IRequestHandler<UpdatePartenaireCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdatePartenaireCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdatePartenaireCommand request, CancellationToken cancellationToken)
    {
        var partenaire = await _context.Partenaires
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (partenaire == null)
        {
            throw new KeyNotFoundException($"Partenaire avec l'Id {request.Id} n'existe pas.");
        }

        // Vérifier l'unicité du code (excluant le partenaire actuel)
        var codeExists = await _context.Partenaires
            .AnyAsync(p => p.Code == request.Code && p.Id != request.Id, cancellationToken);

        if (codeExists)
        {
            throw new InvalidOperationException($"Un partenaire avec le code '{request.Code}' existe déjà.");
        }

        partenaire.Code = request.Code;
        partenaire.Nom = request.Nom;
        partenaire.TypePartenaire = request.TypePartenaire;
        partenaire.Adresse = request.Adresse;
        partenaire.Telephone = request.Telephone;
        partenaire.Email = request.Email;
        partenaire.ContactNom = request.ContactNom;
        partenaire.Observations = request.Observations;
        partenaire.IsActif = request.IsActif;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
