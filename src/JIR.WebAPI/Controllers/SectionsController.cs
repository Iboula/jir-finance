using JIR.Application.Sections.Commands.CreateSection;
using JIR.Application.Sections.Commands.DeleteSection;
using JIR.Application.Sections.Commands.UpdateSection;
using JIR.Application.Sections.Queries.GetSectionById;
using JIR.Application.Sections.Queries.GetSections;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace JIR.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SectionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public SectionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Récupérer toutes les sections
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<SectionDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<SectionDto>>> GetAll()
    {
        var result = await _mediator.Send(new GetSectionsQuery());
        return Ok(result);
    }

    /// <summary>
    /// Récupérer une section par ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(SectionDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SectionDetailDto>> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetSectionByIdQuery(id));
        
        if (result == null)
            return NotFound($"Section avec l'ID {id} introuvable.");

        return Ok(result);
    }

    /// <summary>
    /// Créer une nouvelle section
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateSectionCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    /// <summary>
    /// Mettre à jour une section
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSectionCommand command)
    {
        if (id != command.Id)
            return BadRequest("L'ID de la route ne correspond pas à l'ID de la commande.");

        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Supprimer une section (soft delete)
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteSectionCommand(id));
        return NoContent();
    }
}
