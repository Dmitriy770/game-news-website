using System.Diagnostics;
using GameNews.Articles.Api.Requests;
using GameNews.Articles.Domain.Errors;
using GameNews.Articles.Domain.Models;
using GameNews.Articles.Domain.Services.Interfaces;
using GameNews.Articles.Domain.ValueTypes;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace GameNews.Articles.Api.Controllers;

[ApiController]
[Route("tags")]
public class TagController(
    ITagService tagService
)
{
    [HttpPost]
    public async Task<Results<Ok, BadRequest, ForbidHttpResult>> Create(
        [FromHeader] string userId,
        [FromHeader] string userRole,
        [FromHeader] string username,
        CreateTagRequest request,
        CancellationToken cancellationToken)
    {
        var roleResult = RoleType.Create(userRole);
        if (roleResult.IsFailed)
        {
            return TypedResults.BadRequest();
        }

        var userResult = UserModel.Create(userId, username, roleResult.Value);
        if (userResult.IsFailed)
        {
            return TypedResults.BadRequest();
        }

        var tagResult = TagModel.Create(Guid.NewGuid(), request.Name, request.Description);
        if (tagResult.IsFailed)
        {
            return TypedResults.BadRequest();
        }

        var saveResult = await tagService.Save(tagResult.Value, userResult.Value, cancellationToken);
        if (saveResult.IsSuccess)
        {
            return TypedResults.Ok();
        }
        if (saveResult.HasError<AccessDeniedError>())
        {
            return TypedResults.Forbid();
        }

        throw new UnreachableException();
    }
}