using GameNews.MediaStorage.Domain.Models;

namespace GameNews.MediaStorage.Domain.Services.Interfaces;

public interface IStorageService
{
    public Task<Guid> Save(string contentType, byte[] file, CancellationToken cancellationToken);
    public Task<FileModel> Get(Guid articleId, Guid fileId, CancellationToken cancellationToken);
    public Task<IEnumerable<FileInfo>> GetInfoByArticleId(Guid articleId, CancellationToken cancellationToken);
    public Task Update(Guid articleId, Guid fileId, CancellationToken cancellationToken);
    public Task Delete(Guid articleId, Guid fileId, CancellationToken cancellationToken);
    public Task DeleteAllByArticleId(Guid articleId, CancellationToken cancellationToken);
}