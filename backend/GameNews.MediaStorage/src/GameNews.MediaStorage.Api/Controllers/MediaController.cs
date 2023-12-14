using System.Buffers;
using GameNews.MediaStorage.Api.Requests;
using GameNews.MediaStorage.Api.Responses;
using GameNews.MediaStorage.Domain.Dto;
using GameNews.MediaStorage.Domain.Errors;
using GameNews.MediaStorage.Domain.Models;
using GameNews.MediaStorage.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace GameNews.MediaStorage.Api.Controllers;

[ApiController]
[Route("media-files")]
public sealed class MediaController(
    IStorageService storageService
) : ControllerBase
{
    [HttpPost("{articleId:Guid}")]
    public async Task<Results<Ok<SaveResponse>, ForbidHttpResult>> Save(
        [FromRoute] Guid articleId,
        [FromHeader(Name = "Content-Type")] string type,
        [FromHeader] string userId,
        [FromHeader] string userRole,
        CancellationToken cancellationToken)
    {
        var user = new UserModel(userId, userRole);
        var buffer = (await Request.BodyReader.ReadAsync(cancellationToken)).Buffer;
        var source = buffer.ToArray();
        Request.BodyReader.AdvanceTo(buffer.Start);
        var saveDto = new SaveMediaDto(articleId, Guid.NewGuid(), type, source);

        var result = await storageService.Save(saveDto, user, cancellationToken);

        if (result is { IsSuccess: true, Value: var value })
        {
            return TypedResults.Ok(new SaveResponse(value.articleId, value.mediaId));
        }

        return TypedResults.Forbid();
    }

    [HttpPut("{articleId:Guid}/{mediaId:Guid}/meta")]
    public async Task<Results<Ok, ForbidHttpResult, NotFound>> UpdateMeta(
        [FromRoute] Guid articleId,
        [FromRoute] Guid mediaId,
        [FromHeader] string userId,
        [FromHeader] string userRole,
        UpdateMetaRequest request,
        CancellationToken cancellationToken)
    {
        var user = new UserModel(userId, userRole);
        var meta = new MetaMediaModel(
            articleId,
            mediaId,
            null,
            request.Alt
        );

        var result = await storageService.UpdateMeta(meta, user, cancellationToken);
        if (result.IsSuccess)
        {
            return TypedResults.Ok();
        }

        if (result.HasError<MediaNotFoundError>())
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Forbid();
    }

    [HttpGet("{articleId:Guid}/{mediaId:Guid}")]
    public async Task<Results<FileContentHttpResult, NotFound>> Get(
        [FromRoute] Guid articleId,
        [FromRoute] Guid mediaId,
        CancellationToken cancellationToken)
    {
        var result = await storageService.Get(articleId, mediaId, cancellationToken);
        if (result is { IsSuccess: true, Value: var media })
        {
            return TypedResults.File(media.Source, media.ContentType);
        }

        return TypedResults.NotFound();
    }

    [HttpGet("{articleId:Guid}/{mediaId:Guid}/meta")]
    public async Task<Results<Ok<GetMetaResponse>, NotFound>> GetMeta(
        [FromRoute] Guid articleId,
        [FromRoute] Guid mediaId,
        CancellationToken cancellationToken)
    {
        var result = await storageService.GetMeta(articleId, mediaId, cancellationToken);

        if (result is { IsSuccess: true, Value: var metaMedia })
        {
            return TypedResults.Ok(new GetMetaResponse(
                metaMedia.ArticleId,
                metaMedia.Id,
                metaMedia.Type ?? "",
                metaMedia.Alt ?? ""
            ));
        }

        return TypedResults.NotFound();
    }

    [HttpGet("{articleId:Guid}/meta")]
    public async Task<Ok<IEnumerable<GetMetaResponse>>> GetMetaByArticle(
        [FromRoute] Guid articleId,
        CancellationToken cancellationToken)
    {
        var result = await storageService.GetAllMetaByArticle(articleId, cancellationToken);
        return TypedResults.Ok(result.Select(m => new GetMetaResponse(
            m.ArticleId,
            m.Id,
            m.Type ?? "",
            m.Alt ?? "")));
    }

    [HttpDelete("{articleId:Guid}/{mediaId:Guid}")]
    public async Task<Results<Ok, NotFound, ForbidHttpResult>> Delete(
        [FromRoute] Guid articleId,
        [FromRoute] Guid mediaId,
        [FromHeader] string userId,
        [FromHeader] string userRole,
        CancellationToken cancellationToken)
    {
        var user = new UserModel(userId, userRole);
        var result = await storageService.Delete(articleId, mediaId, user, cancellationToken);
        if (result.IsSuccess)
        {
            return TypedResults.Ok();
        }

        if (result.HasError<AccessDeniedError>())
        {
            return TypedResults.Forbid();
        }

        return TypedResults.NotFound();
    }

    [HttpDelete("{articleId:Guid}")]
    public async Task<Results<Ok, ForbidHttpResult>> DeleteByArticle(
        [FromRoute] Guid articleId,
        [FromHeader] string userId,
        [FromHeader] string userRole,
        CancellationToken cancellationToken)
    {
        var user = new UserModel(userId, userRole);
        var result = await storageService.DeleteAllByArticle(articleId, user, cancellationToken);
        if (result.IsSuccess)
        {
            return TypedResults.Ok();
        }

        return TypedResults.Forbid();
    }
}