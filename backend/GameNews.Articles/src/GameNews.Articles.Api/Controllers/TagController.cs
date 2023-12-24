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
[Route("tags")]
public sealed class TagController(
    ITagService tagService
) : ControllerBase
{
    [HttpPost]
    public async Task<Results<Ok<CreateTagResponse>, BadRequest, ForbidHttpResult>> Create(
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
        if (saveResult is { IsSuccess: true, Value: var value })
        {
            return TypedResults.Ok(new CreateTagResponse(value));
        }

        if (saveResult.HasError<AccessDeniedError>())
        {
            return TypedResults.Forbid();
        }

        throw new UnhandledErrorException();
    }

    [HttpPut("{id:guid}")]
    public async Task<Results<Ok, BadRequest, ForbidHttpResult, NotFound>> Update(
        [FromHeader] string userId,
        [FromHeader] string userRole,
        [FromHeader] string username,
        [FromRoute] Guid id,
        UpdateTagRequest request,
        CancellationToken cancellationToken)
    {
        var roleResult = RoleType.Create(userRole);
        if (roleResult.IsFailed)
        {
            throw new InvalidAuthException();
        }

        var userResult = UserModel.Create(userId, username, roleResult.Value);
        if (userResult.IsFailed)
        {
            throw new InvalidAuthException();
        }

        var tagDto = new UpdateTagDto(id, request.Name, request.Description);
        var updateResult = await tagService.Update(tagDto, userResult.Value, cancellationToken);
        if (updateResult.IsSuccess)
        {
            return TypedResults.Ok();
        }

        if (updateResult.HasError<TagNotFoundError>())
        {
            return TypedResults.NotFound();
        }

        if (updateResult.HasError<ValidateError>())
        {
            return TypedResults.BadRequest();
        }

        if (updateResult.HasError<AccessDeniedError>())
        {
            return TypedResults.Forbid();
        }

        throw new UnhandledErrorException();
    }


    [HttpDelete("{id:guid}")]
    public async Task<Results<Ok, ForbidHttpResult>> Delete(
        [FromHeader] string userId,
        [FromHeader] string userRole,
        [FromHeader] string username,
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var roleResult = RoleType.Create(userRole);
        if (roleResult.IsFailed)
        {
            throw new InvalidAuthException();
        }

        var userResult = UserModel.Create(userId, username, roleResult.Value);
        if (userResult.IsFailed)
        {
            throw new InvalidAuthException();
        }

        var tagResult = await tagService.Delete(id, userResult.Value, cancellationToken);
        if (tagResult.IsSuccess)
        {
            return TypedResults.Ok();
        }

        if (tagResult.HasError<AccessDeniedError>())
        {
            return TypedResults.Forbid();
        }

        throw new UnhandledErrorException();
    }

    [HttpGet("{id:guid}")]
    public async Task<Results<Ok<GetTagResponse>, NotFound>> GetById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var result = await tagService.GetById(id, cancellationToken);
        if (result is { IsSuccess: true, Value: var tag })
        {
            return TypedResults.Ok(new GetTagResponse(tag.Id, tag.Name, tag.Description));
        }

        if (result.HasError<TagNotFoundError>())
        {
            return TypedResults.NotFound();
        }

        throw new UnhandledErrorException();
    }

    [HttpGet]
    public async Task<Results<Ok<GetAllTagResponse>, NotFound>> GetAll(
        CancellationToken cancellationToken)
    {
        var result = new List<Tag>();
        await foreach (var tag in tagService.GetAll(cancellationToken))
        {
            result.Add(new Tag(tag.Id, tag.Name, tag.Description));
        }

        var response = new GetAllTagResponse(result);
        return TypedResults.Ok(response);
    }
}