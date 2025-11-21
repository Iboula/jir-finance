using JIR.Application.Recettes.Commands.CreateRecette;
using JIR.Application.Recettes.Commands.DeleteRecette;
using JIR.Application.Recettes.Commands.UpdateRecette;
using JIR.Application.Recettes.Queries.GetRecetteById;
using JIR.Application.Recettes.Queries.GetRecettes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace JIR.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecettesController : ControllerBase
{
    private readonly IMediator _mediator;

    public RecettesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Récupère la liste des recettes avec filtres optionnels
    /// </summary>
    /// <param name="sectionId">Filtrer par section</param>
    /// <param name="categorie">Filtrer par catégorie</param>
    /// <param name="source">Filtrer par source (recherche partielle)</param>
    /// <param name="dateRecetteDebut">Date de début</param>
    /// <param name="dateRecetteFin">Date de fin</param>
    /// <param name="pageNumber">Numéro de page (défaut: 1)</param>
    /// <param name="pageSize">Taille de page (défaut: 50, max: 100)</param>
    [HttpGet]
    public async Task<IActionResult> GetRecettes(
        [FromQuery] Guid? sectionId,
        [FromQuery] string? categorie,
        [FromQuery] string? source,
        [FromQuery] DateTime? dateRecetteDebut,
        [FromQuery] DateTime? dateRecetteFin,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 50)
    {
        var query = new GetRecettesQuery
        {
            SectionId = sectionId,
            Categorie = categorie,
            Source = source,
            DateRecetteDebut = dateRecetteDebut,
            DateRecetteFin = dateRecetteFin,
            PageNumber = pageNumber,
            PageSize = Math.Min(pageSize, 100)
        };

        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Récupère une recette par son ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetRecetteById(Guid id)
    {
        var query = new GetRecetteByIdQuery { Id = id };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Crée une nouvelle recette
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateRecette([FromBody] CreateRecetteCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetRecetteById), new { id = result }, new { id = result });
    }

    /// <summary>
    /// Met à jour une recette
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRecette(Guid id, [FromBody] UpdateRecetteCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("L'ID de la route ne correspond pas à l'ID de la commande.");
        }

        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Supprime une recette (soft delete)
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRecette(Guid id)
    {
        var command = new DeleteRecetteCommand { Id = id };
        await _mediator.Send(command);
        return NoContent();
    }
}
