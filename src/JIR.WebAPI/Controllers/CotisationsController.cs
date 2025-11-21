using JIR.Application.Cotisations.Commands.CreateCotisation;
using JIR.Application.Cotisations.Commands.DeleteCotisation;
using JIR.Application.Cotisations.Commands.UpdateCotisation;
using JIR.Application.Cotisations.Queries.GetCotisationById;
using JIR.Application.Cotisations.Queries.GetCotisations;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace JIR.WebAPI.Controllers;

/// <summary>
/// Controller pour la gestion des cotisations
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CotisationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public CotisationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Récupère toutes les cotisations avec filtres optionnels
    /// </summary>
    /// <param name="sectionId">ID de la section (optionnel)</param>
    /// <param name="periode">Période au format YYYY-MM (optionnel)</param>
    /// <param name="datePaiementDebut">Date de paiement début (optionnel)</param>
    /// <param name="datePaiementFin">Date de paiement fin (optionnel)</param>
    /// <param name="pageNumber">Numéro de page (défaut: 1)</param>
    /// <param name="pageSize">Taille de page (défaut: 50)</param>
    [HttpGet]
    public async Task<ActionResult<List<CotisationDto>>> GetAll(
        [FromQuery] Guid? sectionId = null,
        [FromQuery] string? periode = null,
        [FromQuery] DateTime? datePaiementDebut = null,
        [FromQuery] DateTime? datePaiementFin = null,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 50)
    {
        var query = new GetCotisationsQuery
        {
            SectionId = sectionId,
            Periode = periode,
            DatePaiementDebut = datePaiementDebut,
            DatePaiementFin = datePaiementFin,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var cotisations = await _mediator.Send(query);
        return Ok(cotisations);
    }

    /// <summary>
    /// Récupère une cotisation par son ID
    /// </summary>
    /// <param name="id">ID de la cotisation</param>
    [HttpGet("{id}")]
    public async Task<ActionResult<CotisationDetailDto>> GetById(Guid id)
    {
        var query = new GetCotisationByIdQuery { Id = id };
        var cotisation = await _mediator.Send(query);
        return Ok(cotisation);
    }

    /// <summary>
    /// Crée une nouvelle cotisation
    /// </summary>
    /// <param name="command">Données de la cotisation</param>
    [HttpPost]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateCotisationCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    /// <summary>
    /// Met à jour une cotisation existante
    /// </summary>
    /// <param name="id">ID de la cotisation</param>
    /// <param name="command">Nouvelles données de la cotisation</param>
    [HttpPut("{id}")]
    public async Task<ActionResult> Update(Guid id, [FromBody] UpdateCotisationCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("L'ID dans l'URL ne correspond pas à l'ID de la cotisation.");
        }

        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Supprime une cotisation (soft delete)
    /// </summary>
    /// <param name="id">ID de la cotisation</param>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var command = new DeleteCotisationCommand { Id = id };
        await _mediator.Send(command);
        return NoContent();
    }
}
