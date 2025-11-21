using JIR.Application.Common.Interfaces;
using JIR.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JIR.Application.Partenaires.Commands.CreatePartenaire;

public class CreatePartenaireCommandHandler : IRequestHandler<CreatePartenaireCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreatePartenaireCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreatePartenaireCommand request, CancellationToken cancellationToken)
    {
        // Vérifier que le code n'existe pas déjà
        var codeExists = await _context.Partenaires
            .AnyAsync(p => p.Code == request.Code && !p.IsDeleted, cancellationToken);

        if (codeExists)
        {
            throw new Exception($"Un partenaire avec le code {request.Code} existe déjà.");
        }

        var partenaire = new Partenaire
        {
            Id = Guid.NewGuid(),
            Code = request.Code,
            Nom = request.Nom,
            TypePartenaire = request.TypePartenaire,
            Adresse = request.Adresse,
            Telephone = request.Telephone,
            Email = request.Email,
            ContactNom = request.ContactNom,
            Observations = request.Observations,
            IsActif = request.IsActif,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "System"
        };

        _context.Partenaires.Add(partenaire);
        await _context.SaveChangesAsync(cancellationToken);

        return partenaire.Id;
    }
}
