using FluentResults;
using GameNews.MediaStorage.Domain.Dto;
using GameNews.MediaStorage.Domain.Errors;
using GameNews.MediaStorage.Domain.Interfaces;
using GameNews.MediaStorage.Domain.Models;
using GameNews.MediaStorage.Domain.Services.Interfaces;

namespace GameNews.MediaStorage.Domain.Services;

public sealed class StorageService(
    IStorageRepository storageRepository
) : IStorageService
{
    public async Task<Result<(Guid articleId, Guid mediaId)>> Save(
        SaveMediaDto saveMediaDto,
        UserModel user,
        CancellationToken cancellationToken
    )
    {
        if (user.Role is not ("Author" or "Administrator"))
        {
            return Result.Fail(new AccessDeniedError());
        }

        var (articleId, mediaId, type, source) = saveMediaDto;
        await storageRepository.Save(articleId, mediaId, type, source, cancellationToken);

        return (articleId, mediaId);
    }

    public async Task<Result> UpdateMeta(
        MetaMediaModel metaMedia,
        UserModel user,
        CancellationToken cancellationToken
    )
    {
        if (user.Role is not ("Author" or "Administrator"))
        {
            return Result.Fail(new AccessDeniedError());
        }

        return await storageRepository.UpdateMeta(
            metaMedia,
            cancellationToken
        );
    }

    public async Task<Result<MediaModel>> Get(
        Guid articleId,
        Guid mediaId,
        CancellationToken cancellationToken
    ) => await storageRepository.Get(
        articleId,
        mediaId,
        cancellationToken
    );

    public async Task<Result<MetaMediaModel>> GetMeta(
        Guid articleId,
        Guid mediaId,
        CancellationToken cancellationToken
    ) => await storageRepository.GetMeta(
        articleId,
        mediaId,
        cancellationToken
    );

    public async Task<IEnumerable<MetaMediaModel>> GetAllMetaByArticle(
        Guid articleId,
        CancellationToken cancellationToken
    ) => await storageRepository.GetAllMetaByArticle(
        articleId,
        cancellationToken
    );

    public async Task<Result> Delete(
        Guid articleId,
        Guid mediaId,
        UserModel user,
        CancellationToken cancellationToken
    )
    {
        if (user.Role is not ("Author" or "Administrator"))
        {
            return Result.Fail(new AccessDeniedError());
        }

        return await storageRepository.Delete(articleId, mediaId, cancellationToken);
    }

    public async Task<Result> DeleteAllByArticle(
        Guid articleId,
        UserModel user,
        CancellationToken cancellationToken
    )
    {
        if (user.Role is not ("Author" or "Administrator"))
        {
            return Result.Fail(new AccessDeniedError());
        }

        await storageRepository.DeleteAllByArticle(articleId, cancellationToken);

        return Result.Ok();
    }
}