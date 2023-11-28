using GameNews.MediaStorage.Domain.Errors;
using GameNews.MediaStorage.Domain.Interfaces;
using GameNews.MediaStorage.Domain.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using OneOf;

namespace GameNews.MediaStorage.Infrastructure.Repositories;

public sealed class StorageRepository : IStorageRepository
{
    private readonly GridFSBucket _bucket;

    public StorageRepository()
    {
        const string connectionUri = "mongodb:://localhost:27017";
        var client = new MongoClient(connectionUri);

        _bucket = new GridFSBucket(client.GetDatabase("game-news-media"), new GridFSBucketOptions
        {
            BucketName = "game-news-media"
        });
    }

    public async Task<Guid> Save(Guid articleId, string contentType, byte[] source, CancellationToken cancellationToken)
    {
        var fileId = new Guid();
        var filename = $"{articleId}.{fileId}";

        var options = new GridFSUploadOptions
        {
            Metadata = new BsonDocument
            {
                { "contentType", contentType }
            }
        };
        await _bucket.UploadFromBytesAsync(filename, source, options, cancellationToken);

        return fileId;
    }

    public async Task<OneOf<FileModel, FileNotFoundError>> Get(Guid articleId, Guid fileId,
        CancellationToken cancellationToken)
    {
        var fileInfo = await FindFileInfo(articleId, fileId, cancellationToken);
        if (fileInfo is null)
        {
            return new FileNotFoundError();
        }

        var source = await _bucket.DownloadAsBytesAsync(fileInfo.Id, cancellationToken: cancellationToken);
        return new FileModel(
            articleId,
            fileId,
            fileInfo.Metadata["contentType"].AsString,
            source
        );
    }

    public async Task<IEnumerable<FileInfoModel>> GetInfoByArticleId(Guid articleId, CancellationToken cancellationToken)
    {
        return (await FindFileByArticleId(articleId, cancellationToken)).Select(
            f => new FileInfoModel(
                articleId,
                new Guid(f.Filename.Split(".")[1]),
                f.Metadata["contentType"].AsString
            ));
    }

    public async Task<OneOf<Guid, FileNotFoundError>> Update(Guid articleId, Guid fileId, string contentType,
        byte[] source, CancellationToken cancellationToken)
    {
        var fileInfo = await FindFileInfo(articleId, fileId, cancellationToken);
        if (fileInfo is null)
        {
            return new FileNotFoundError();
        }

        var options = new GridFSUploadOptions
        {
            Metadata = new BsonDocument
            {
                { "contentType", contentType }
            }
        };
        await _bucket.UploadFromBytesAsync($"{articleId}.{fileId}", source, options, cancellationToken);

        return fileId;
    }

    public async Task<OneOf<Guid, FileNotFoundError>> Delete(Guid articleId, Guid fileId,
        CancellationToken cancellationToken)
    {
        var fileInfo = await FindFileInfo(articleId, fileId, cancellationToken);
        if (fileInfo is null)
        {
            return new FileNotFoundError();
        }

        await _bucket.DeleteAsync(fileInfo.Id, cancellationToken);

        return fileId;
    }

    public async Task DeleteAllByArticleId(Guid articleId, CancellationToken cancellationToken)
    {
        foreach (var fileInfo in await FindFileByArticleId(articleId, cancellationToken))
        {
            await _bucket.DeleteAsync(fileInfo.Id, cancellationToken);
        }
    }

    private async Task<GridFSFileInfo?> FindFileInfo(
        Guid articleId,
        Guid fileId,
        CancellationToken cancellationToken = default)
    {
        var filename = $"{articleId}.{fileId}";

        var filter = Builders<GridFSFileInfo>.Filter.Eq(f => f.Filename, filename);
        var sort = Builders<GridFSFileInfo>.Sort.Descending(f => f.UploadDateTime);
        var options = new GridFSFindOptions
        {
            Limit = 1,
            Sort = sort
        };

        using var cursor = await _bucket.FindAsync(filter, options, cancellationToken);
        return (await cursor.ToListAsync(cancellationToken)).FirstOrDefault();
    }

    private async Task<IEnumerable<GridFSFileInfo>> FindFileByArticleId(
        Guid articleId,
        CancellationToken cancellationToken = default)
    {
        var filter = Builders<GridFSFileInfo>.Filter.Exists(f => f.Filename.StartsWith(articleId.ToString()));

        using var cursor = await _bucket.FindAsync(filter, cancellationToken: cancellationToken);
        return await cursor.ToListAsync(cancellationToken);
    }
}