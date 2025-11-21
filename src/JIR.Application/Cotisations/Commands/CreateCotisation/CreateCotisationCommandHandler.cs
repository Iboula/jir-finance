using JIR.Application.Common.Interfaces;
using JIR.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JIR.Application.Cotisations.Commands.CreateCotisation;

public class CreateCotisationCommandHandler : IRequestHandler<CreateCotisationCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<CreateCotisationCommandHandler> _logger;

    public CreateCotisationCommandHandler(IApplicationDbContext context, ILogger<CreateCotisationCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Guid> Handle(CreateCotisationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Début de la création de cotisation pour SectionId: {SectionId}", request.SectionId);

            // Vérifier que la section existe
            var sectionExists = await _context.Sections
                .AnyAsync(s => s.Id == request.SectionId && !s.IsDeleted, cancellationToken);

            _logger.LogInformation("Vérification section: {SectionExists}", sectionExists);

            if (!sectionExists)
            {
                throw new Exception($"La section avec l'ID {request.SectionId} n'existe pas.");
            }

            var cotisation = new Cotisation
            {
                Id = Guid.NewGuid(),
                SectionId = request.SectionId,
                MembreNom = request.MembreNom,
                MembreMatricule = request.MembreMatricule,
                Montant = request.Montant,
                Periode = request.Periode,
                DatePaiement = DateTime.SpecifyKind(request.DatePaiement, DateTimeKind.Utc),
                ModePaiement = request.ModePaiement,
                RecuNumero = request.RecuNumero,
                Statut = request.Statut,
                Observations = request.Observations
            };

            _logger.LogInformation("Ajout de la cotisation Id: {CotisationId}", cotisation.Id);

            _context.Cotisations.Add(cotisation);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Cotisation créée avec succès: {CotisationId}", cotisation.Id);

            return cotisation.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de la création de la cotisation: {Message}", ex.Message);
            throw;
        }
    }
}
