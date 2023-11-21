using GameNews.MediaStorage.Domain.Models;
using GameNews.MediaStorage.Domain.Services.Interfaces;

namespace GameNews.MediaStorage.Infrastructure.Repositories;

public class StorageRepository : IStorageService
{
    public Task<Guid> Save(string contentType, byte[] file, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<FileModel> Get(Guid articleId, Guid fileId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<FileInfo>> GetInfoByArticleId(Guid articleId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task Update(Guid articleId, Guid fileId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task Delete(Guid articleId, Guid fileId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task DeleteByArticleId(Guid articleId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}