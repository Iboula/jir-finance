using JIR.Application.MouvementsStock.Commands.CreateMouvementStock;
using JIR.Application.MouvementsStock.Commands.DeleteMouvementStock;
using JIR.Application.MouvementsStock.Queries.GetMouvementStockById;
using JIR.Application.MouvementsStock.Queries.GetMouvementsStock;
using JIR.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JIR.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
// [Authorize] // Desactive temporairement pour les tests
public class MouvementsStockController : ControllerBase
{
    private readonly IMediator _mediator;

    public MouvementsStockController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<GetMouvementsStockResponse>> GetMouvementsStock(
        [FromQuery] Guid? articleId,
        [FromQuery] Guid? magasinId,
        [FromQuery] TypeMouvement? typeMouvement,
        [FromQuery] DateTime? dateDebut,
        [FromQuery] DateTime? dateFin,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 50)
    {
        var query = new GetMouvementsStockQuery
        {
            ArticleId = articleId,
            MagasinId = magasinId,
            TypeMouvement = typeMouvement,
            DateDebut = dateDebut,
            DateFin = dateFin,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MouvementStockDetailDto>> GetMouvementStockById(Guid id)
    {
        var result = await _mediator.Send(new GetMouvementStockByIdQuery(id));

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateMouvementStock([FromBody] CreateMouvementStockCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetMouvementStockById), new { id }, id);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMouvementStock(Guid id)
    {
        await _mediator.Send(new DeleteMouvementStockCommand(id));
        return NoContent();
    }
}
