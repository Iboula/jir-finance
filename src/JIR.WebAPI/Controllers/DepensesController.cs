using JIR.Application.Depenses.Commands.CreateDepense;
using JIR.Application.Depenses.Commands.DeleteDepense;
using JIR.Application.Depenses.Commands.PayerDepense;
using JIR.Application.Depenses.Commands.RejeterDepense;
using JIR.Application.Depenses.Commands.UpdateDepense;
using JIR.Application.Depenses.Commands.ValiderDepense;
using JIR.Application.Depenses.Queries.GetDepenseById;
using JIR.Application.Depenses.Queries.GetDepenses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace JIR.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepensesController : ControllerBase
{
    private readonly IMediator _mediator;

    public DepensesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Récupère la liste des dépenses avec filtres optionnels
    /// </summary>
    /// <param name="sectionId">Filtrer par section</param>
    /// <param name="statut">Filtrer par statut (Brouillon, Valide, Rejete, Paye)</param>
    /// <param name="dateDepenseDebut">Date de début</param>
    /// <param name="dateDepenseFin">Date de fin</param>
    /// <param name="pageNumber">Numéro de page (défaut: 1)</param>
    /// <param name="pageSize">Taille de page (défaut: 50, max: 100)</param>
    [HttpGet]
    public async Task<IActionResult> GetDepenses(
        [FromQuery] Guid? sectionId,
        [FromQuery] string? statut,
        [FromQuery] DateTime? dateDepenseDebut,
        [FromQuery] DateTime? dateDepenseFin,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 50)
    {
        var query = new GetDepensesQuery
        {
            SectionId = sectionId,
            Statut = statut,
            DateDepenseDebut = dateDepenseDebut,
            DateDepenseFin = dateDepenseFin,
            PageNumber = pageNumber,
            PageSize = Math.Min(pageSize, 100)
        };

        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Récupère une dépense par son ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetDepenseById(Guid id)
    {
        var query = new GetDepenseByIdQuery { Id = id };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Crée une nouvelle dépense (statut initial: Brouillon)
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateDepense([FromBody] CreateDepenseCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetDepenseById), new { id = result }, new { id = result });
    }

    /// <summary>
    /// Met à jour une dépense (uniquement si statut = Brouillon)
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDepense(Guid id, [FromBody] UpdateDepenseCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("L'ID de la route ne correspond pas à l'ID de la commande.");
        }

        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Supprime une dépense (uniquement si statut = Brouillon, soft delete)
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDepense(Guid id)
    {
        var command = new DeleteDepenseCommand { Id = id };
        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Valide une dépense (Brouillon → Valide)
    /// </summary>
    [HttpPut("{id}/valider")]
    public async Task<IActionResult> ValiderDepense(Guid id, [FromBody] ValiderDepenseCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("L'ID de la route ne correspond pas à l'ID de la commande.");
        }

        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Rejette une dépense (Brouillon/EnAttenteValidation → Rejete)
    /// </summary>
    [HttpPut("{id}/rejeter")]
    public async Task<IActionResult> RejeterDepense(Guid id, [FromBody] RejeterDepenseCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("L'ID de la route ne correspond pas à l'ID de la commande.");
        }

        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Marque une dépense comme payée (Valide → Paye)
    /// </summary>
    [HttpPut("{id}/payer")]
    public async Task<IActionResult> PayerDepense(Guid id, [FromBody] PayerDepenseCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("L'ID de la route ne correspond pas à l'ID de la commande.");
        }

        await _mediator.Send(command);
        return NoContent();
    }
}
