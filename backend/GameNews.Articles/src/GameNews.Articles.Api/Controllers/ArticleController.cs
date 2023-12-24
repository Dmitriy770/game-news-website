using GameNews.Articles.Api.Exceptions;
using GameNews.Articles.Api.Requests;
using GameNews.Articles.Api.Responses;
using GameNews.Articles.Domain.DTOs;
using GameNews.Articles.Domain.Errors;
using GameNews.Articles.Domain.Models;
using GameNews.Articles.Domain.Models.ValueTypes;
using GameNews.Articles.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace GameNews.Articles.Api.Controllers;

[ApiController]
[Route("articles")]
public sealed class ArticleController(
    IArticleService articleService
) : ControllerBase
{
    [HttpPost]
    public async Task<Results<Ok<CreateArticleResponse>, BadRequest, ForbidHttpResult>> Create(
        [FromHeader] string userId,
        [FromHeader] string userRole,
        [FromHeader] string username,
        CancellationToken cancellationToken
    )
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

        var articleResult = ArticleModel.Create(Guid.NewGuid(), DateTime.Now, userResult.Value.Id);
        if (articleResult.IsFailed)
        {
            return TypedResults.BadRequest();
        }

        var createResult = await articleService.Create(articleResult.Value, userResult.Value, cancellationToken);
        if (createResult is { IsSuccess: true, Value: var value })
        {
            return TypedResults.Ok(new CreateArticleResponse(value.Id, value.CreationDate, value.AuthorId));
        }

        if (createResult.HasError<AccessDeniedError>())
        {
            return TypedResults.Forbid();
        }

        throw new UnhandledErrorException();
    }

    [HttpPut("id:guid")]
    public async Task<Results<Ok, BadRequest, ForbidHttpResult>> Update(
        [FromRoute] Guid id,
        [FromHeader] string userId,
        [FromHeader] string userRole,
        [FromHeader] string username,
        UpdateArticleRequest request,
        CancellationToken cancellationToken
    )
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

        var updateDto = new UpdateArticleDto(
            id,
            request.Title,
            request.PreviewText,
            request.PreviewMediaId,
            request.Tags,
            request.Content
        );

        var updateResult = await articleService.Update(updateDto, userResult.Value, cancellationToken);
        if (updateResult is { IsSuccess: true })
        {
            return TypedResults.Ok();
        }

        if (updateResult.HasError<AccessDeniedError>())
        {
            return TypedResults.Forbid();
        }

        throw new UnhandledErrorException();
    }

    [HttpDelete("id:guid")]
    public async Task<Results<Ok, BadRequest, ForbidHttpResult>> Delete(
        [FromRoute] Guid id,
        [FromHeader] string userId,
        [FromHeader] string userRole,
        [FromHeader] string username,
        CancellationToken cancellationToken
    )
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

        var deleteResult = await articleService.Delete(id, userResult.Value, cancellationToken);
        if (deleteResult is { IsSuccess: true })
        {
            return TypedResults.Ok();
        }

        if (deleteResult.HasError<AccessDeniedError>())
        {
            return TypedResults.Forbid();
        }

        throw new UnhandledErrorException();
    }

    [HttpGet("id:guid")]
    public async Task<Results<Ok<GetArticleResponse>, BadRequest, ForbidHttpResult>> GetById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken
    )
    {
        var result = await articleService.GetArticle(id, cancellationToken);
        if (result is { IsSuccess: true, Value: var value })
        {
            return TypedResults.Ok(new GetArticleResponse(
                value.Id,
                value.Title,
                value.CreationDate,
                value.AuthorId,
                value.IsVisible,
                value.PreviewText,
                value.PreviewMediaId,
                value.Tags.Select(t => new Tag(t.Id, t.Name, t.Description)).ToList(),
                value.Content));
        }

        throw new UnhandledErrorException();
    }
}