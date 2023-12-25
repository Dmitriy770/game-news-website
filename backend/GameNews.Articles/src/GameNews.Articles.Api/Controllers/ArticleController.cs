using GameNews.Articles.Api.Extensions;
using GameNews.Articles.Api.Requests.Articles;
using GameNews.Articles.Application.Commands.Articles;
using GameNews.Articles.Application.Queries.Articles;
using GameNews.Articles.Application.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GameNews.Articles.Api.Controllers;

[ApiController]
[Route("articles")]
public sealed class ArticleController(
    ISender sender
) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromHeader] string userId,
        [FromHeader] string username,
        [FromHeader] string userRole,
        CancellationToken cancellationToken
    )
    {
        var result = await sender.Send(new CreateArticleCommand(
                Guid.NewGuid(),
                DateTime.UtcNow,
                new User(userId, username, userRole)
            ),
            cancellationToken);

        return result.IsSuccess ? new OkObjectResult(result.Value) : result.Errors.ToActionResult();
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        [FromRoute] Guid id,
        [FromHeader] string userId,
        [FromHeader] string username,
        [FromHeader] string userRole,
        UpdateArticleRequest request,
        CancellationToken cancellationToken
    )
    {
        var result = await sender.Send(new UpdateArticleCommand(
            id,
            request.Title,
            request.Content,
            request.Tags,
            new User(userId, username, userRole)
        ), cancellationToken);

        return result.IsSuccess ? new OkResult() : result.Errors.ToActionResult();
    }

    [HttpPut("{id:guid}/preview")]
    public async Task<IActionResult> UpdatePreview(
        [FromRoute] Guid id,
        [FromHeader] string userId,
        [FromHeader] string username,
        [FromHeader] string userRole,
        UpdatePreviewArticleRequest request,
        CancellationToken cancellationToken
    )
    {
        var result = await sender.Send(new UpdatePreviewArticleCommand(
            id,
            request.PreviewMediaId,
            request.PreviewText,
            new User(userId, username, userRole)
        ), cancellationToken);

        return result.IsSuccess ? new OkResult() : result.Errors.ToActionResult();
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> ChangeVisible(
        [FromRoute] Guid id,
        [FromQuery] bool isVisible,
        [FromHeader] string userId,
        [FromHeader] string username,
        [FromHeader] string userRole,
        CancellationToken cancellationToken
    )
    {
        var result = await sender.Send(new ChangeVisibilityArticleCommand(
            id,
            isVisible,
            new User(userId, username, userRole)
        ), cancellationToken);

        return result.IsSuccess ? new OkResult() : result.Errors.ToActionResult();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid id,
        [FromHeader] string userId,
        [FromHeader] string username,
        [FromHeader] string userRole,
        CancellationToken cancellationToken
    )
    {
        var result = await sender.Send(new DeleteArticleCommand(
            id,
            new User(userId, username, userRole)
        ), cancellationToken);

        return result.IsSuccess ? new OkResult() : result.Errors.ToActionResult();
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        [FromRoute] Guid id,
        [FromHeader] string userId,
        [FromHeader] string username,
        [FromHeader] string userRole,
        CancellationToken cancellationToken
    )
    {
        var result = await sender.Send(new GetArticleQuery(
            id,
            new User(userId, username, userRole)
        ), cancellationToken);

        return result.IsSuccess ? new OkObjectResult(result.Value) : result.Errors.ToActionResult();
    }

    [HttpGet]
    public async Task<IActionResult> GetPreviews(
        [FromQuery] int skip,
        [FromQuery] int take,
        [FromQuery] string? query,
        [FromQuery] bool isVisible,
        [FromHeader] string userId,
        [FromHeader] string username,
        [FromHeader] string userRole,
        CancellationToken cancellationToken
    )
    {
        var result = await sender.Send(new GetPreviewsQuery(
            skip,
            take,
            isVisible,
            query,
            new User(userId, username, userRole)
        ), cancellationToken);

        return new OkObjectResult(result);
    }
}