using GameNews.Articles.Api.Extensions;
using GameNews.Articles.Api.Requests.Tags;
using GameNews.Articles.Application.Commands.Tags;
using GameNews.Articles.Application.Queries.Tags;
using GameNews.Articles.Application.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GameNews.Articles.Api.Controllers;

[ApiController]
[Route("tags")]
public sealed class TagController(
    ISender sender
) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Add(
        [FromHeader] string userId,
        [FromHeader] string username,
        [FromHeader] string userRole,
        AddTagRequest request,
        CancellationToken cancellationToken
    )
    {
        var result = await sender.Send(new AddTagCommand(
                new Tag(Guid.NewGuid(), request.Name, request.Description),
                new User(userId, username, userRole)
            ),
            cancellationToken
        );

        return result.IsSuccess ? new OkObjectResult(result.Value) : result.Errors.ToActionResult();
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        [FromRoute] Guid id,
        [FromHeader] string userId,
        [FromHeader] string username,
        [FromHeader] string userRole,
        UpdateTagRequest request,
        CancellationToken cancellationToken
    )
    {
        var result = await sender.Send(new UpdateTagCommand(
                new Tag(id, request.Name, request.Description),
                new User(userId, username, userRole)
            ),
            cancellationToken
        );

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
        var result = await sender.Send(new DeleteTagCommand(
                id,
                new User(userId, username, userRole)
            ),
            cancellationToken
        );

        return result.IsSuccess ? new OkResult() : result.Errors.ToActionResult();
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken
    )
    {
        var result = await sender.Send(new GetTagQuery(id), cancellationToken);

        return result.IsSuccess ? new OkObjectResult(result.Value) : result.Errors.ToActionResult();
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll(
        CancellationToken cancellationToken
    )
    {
        var result = await sender.Send(new GetAllTagsQuery(), cancellationToken);

        return new OkObjectResult(result);
    }
}