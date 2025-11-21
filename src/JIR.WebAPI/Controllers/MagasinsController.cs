using JIR.Application.Magasins.Commands.CreateMagasin;
using JIR.Application.Magasins.Commands.DeleteMagasin;
using JIR.Application.Magasins.Commands.UpdateMagasin;
using JIR.Application.Magasins.Queries.GetMagasinById;
using JIR.Application.Magasins.Queries.GetMagasins;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace JIR.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MagasinsController : ControllerBase
{
    private readonly IMediator _mediator;

    public MagasinsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtenir la liste des magasins avec recherche optionnelle
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<GetMagasinsQueryResult>> GetMagasins(
        [FromQuery] string? search,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 50)
    {
        var query = new GetMagasinsQuery
        {
            Search = search,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Obtenir un magasin par son identifiant
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<JIR.Application.Magasins.Queries.Common.MagasinDetailDto>> GetMagasinById(Guid id)
    {
        var query = new GetMagasinByIdQuery { Id = id };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Créer un nouveau magasin
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateMagasin([FromBody] CreateMagasinCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetMagasinById), new { id = result }, result);
    }

    /// <summary>
    /// Mettre à jour un magasin existant
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateMagasin(Guid id, [FromBody] UpdateMagasinCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("L'identifiant dans l'URL ne correspond pas à celui du corps de la requête.");
        }

        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Supprimer un magasin (soft delete)
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteMagasin(Guid id)
    {
        var command = new DeleteMagasinCommand { Id = id };
        await _mediator.Send(command);
        return NoContent();
    }
}
