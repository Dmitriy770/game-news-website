using GameNews.MediaStorage.Domain.Errors;
using GameNews.MediaStorage.Domain.Models;
using OneOf;

namespace GameNews.MediaStorage.Domain.Interfaces;

public interface IStorageRepository
{
    public Task<Guid> Save(Guid articleId, string contentType, byte[] source, CancellationToken cancellationToken);
    public Task<OneOf<FileModel, FileNotFoundError>> Get(Guid articleId, Guid fileId, CancellationToken cancellationToken);
    public Task<IEnumerable<FileInfoModel>> GetInfoByArticleId(Guid articleId, CancellationToken cancellationToken);
    public Task<OneOf<Guid, FileNotFoundError>> Update(Guid articleId, Guid fileId, string contentType, byte[] source, CancellationToken cancellationToken);
    public Task<OneOf<Guid, FileNotFoundError>> Delete(Guid articleId, Guid fileId, CancellationToken cancellationToken);
    public Task DeleteAllByArticleId(Guid articleId, CancellationToken cancellationToken);
}