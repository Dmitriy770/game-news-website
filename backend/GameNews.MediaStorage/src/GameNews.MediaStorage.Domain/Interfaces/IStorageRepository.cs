using FluentResults;
using GameNews.MediaStorage.Domain.Models;

namespace GameNews.MediaStorage.Domain.Interfaces;

public interface IStorageRepository
{
    public Task Save(
        Guid articleId,
        Guid mediaId,
        string type,
        byte[] source,
        CancellationToken cancellationToken);

    public Task<Result<MediaModel>> Get(
        Guid articleId,
        Guid mediaId,
        CancellationToken cancellationToken);

    public Task<IEnumerable<MetaMediaModel>> GetAllMetaByArticle(
        Guid articleId,
        CancellationToken cancellationToken);

    public Task<Result> UpdateMeta(
        MetaMediaModel metaMedia,
        CancellationToken cancellationToken);

    public Task<Result<MetaMediaModel>> GetMeta(
        Guid articleId,
        Guid mediaId,
        CancellationToken cancellationToken
    );

    public Task<Result> Delete(
        Guid articleId,
        Guid mediaId,
        CancellationToken cancellationToken);

    public Task DeleteAllByArticle(
        Guid articleId,
        CancellationToken cancellationToken);
}