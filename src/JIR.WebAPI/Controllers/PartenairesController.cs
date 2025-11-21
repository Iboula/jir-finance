using JIR.Application.Partenaires.Commands.CreatePartenaire;
using JIR.Application.Partenaires.Commands.DeletePartenaire;
using JIR.Application.Partenaires.Commands.UpdatePartenaire;
using JIR.Application.Partenaires.Queries.GetPartenaireById;
using JIR.Application.Partenaires.Queries.GetPartenaires;
using JIR.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace JIR.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PartenairesController : ControllerBase
{
    private readonly IMediator _mediator;

    public PartenairesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtenir la liste des partenaires avec filtres optionnels
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<GetPartenairesQueryResult>> GetPartenaires(
        [FromQuery] TypePartenaire? typePartenaire,
        [FromQuery] bool? isActif,
        [FromQuery] string? search,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 50)
    {
        var query = new GetPartenairesQuery
        {
            TypePartenaire = typePartenaire,
            IsActif = isActif,
            Search = search,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Obtenir un partenaire par son identifiant
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<JIR.Application.Partenaires.Queries.Common.PartenaireDetailDto>> GetPartenaireById(Guid id)
    {
        var query = new GetPartenaireByIdQuery { Id = id };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Créer un nouveau partenaire
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<Guid>> CreatePartenaire([FromBody] CreatePartenaireCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetPartenaireById), new { id = result }, result);
    }

    /// <summary>
    /// Mettre à jour un partenaire existant
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdatePartenaire(Guid id, [FromBody] UpdatePartenaireCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("L'identifiant dans l'URL ne correspond pas à celui du corps de la requête.");
        }

        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Supprimer un partenaire (soft delete)
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePartenaire(Guid id)
    {
        var command = new DeletePartenaireCommand { Id = id };
        await _mediator.Send(command);
        return NoContent();
    }
}
