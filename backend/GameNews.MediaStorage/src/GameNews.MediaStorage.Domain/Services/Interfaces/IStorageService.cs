using FluentResults;
using GameNews.MediaStorage.Domain.Dto;
using GameNews.MediaStorage.Domain.Models;

namespace GameNews.MediaStorage.Domain.Services.Interfaces;

public interface IStorageService
{
    public Task<Result<(Guid articleId, Guid mediaId)>> Save(
        SaveMediaDto saveMediaDto,
        UserModel user,
        CancellationToken cancellationToken
    );

    public Task<Result> UpdateMeta(
        MetaMediaModel metaMedia,
        UserModel user,
        CancellationToken cancellationToken
    );

    public Task<Result<MediaModel>> Get(
        Guid articleId,
        Guid mediaId,
        CancellationToken cancellationToken
    );

    public Task<Result<MetaMediaModel>> GetMeta(
        Guid articleId,
        Guid mediaId,
        CancellationToken cancellationToken
    );

    public Task<IEnumerable<MetaMediaModel>> GetAllMetaByArticle(
        Guid articleId,
        CancellationToken cancellationToken
    );

    public Task<Result> Delete(
        Guid articleId,
        Guid mediaId,
        UserModel user,
        CancellationToken cancellationToken
    );

    public Task<Result> DeleteAllByArticle(
        Guid articleId,
        UserModel user,
        CancellationToken cancellationToken
    );
}