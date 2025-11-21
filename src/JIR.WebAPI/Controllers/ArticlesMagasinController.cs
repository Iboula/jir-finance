using JIR.Application.ArticlesMagasin.Commands.CreateArticleMagasin;
using JIR.Application.ArticlesMagasin.Commands.DeleteArticleMagasin;
using JIR.Application.ArticlesMagasin.Commands.UpdateArticleMagasin;
using JIR.Application.ArticlesMagasin.Queries.GetArticleMagasinById;
using JIR.Application.ArticlesMagasin.Queries.GetArticlesMagasin;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace JIR.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArticlesMagasinController : ControllerBase
{
    private readonly IMediator _mediator;

    public ArticlesMagasinController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtenir la liste des articles avec filtres optionnels
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<GetArticlesMagasinQueryResult>> GetArticles(
        [FromQuery] Guid? magasinId,
        [FromQuery] string? search,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 50)
    {
        var query = new GetArticlesMagasinQuery
        {
            MagasinId = magasinId,
            Search = search,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Obtenir un article par son identifiant
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<JIR.Application.ArticlesMagasin.Queries.Common.ArticleMagasinDetailDto>> GetArticleById(Guid id)
    {
        var query = new GetArticleMagasinByIdQuery { Id = id };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Créer un nouvel article
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateArticle([FromBody] CreateArticleMagasinCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetArticleById), new { id = result }, result);
    }

    /// <summary>
    /// Mettre à jour un article existant
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateArticle(Guid id, [FromBody] UpdateArticleMagasinCommand command)
    {
        command.Id = id;
        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Supprimer un article (soft delete)
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteArticle(Guid id)
    {
        var command = new DeleteArticleMagasinCommand { Id = id };
        await _mediator.Send(command);
        return NoContent();
    }
}
