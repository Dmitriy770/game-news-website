using FluentResults;
using GameNews.MediaStorage.Domain.Errors;
using GameNews.MediaStorage.Domain.Interfaces;
using GameNews.MediaStorage.Domain.Models;
using GameNews.MediaStorage.Infrastructure.Options;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace GameNews.MediaStorage.Infrastructure.Repositories;

public sealed class StorageRepository : IStorageRepository
{
    private readonly GridFSBucket _bucket;

    public StorageRepository(IOptions<MongoOptions> mongoOptions)
    {
        var (user, password) = mongoOptions.Value;
        var connectionString = $"mongodb://{user}:{password}@storage-db:27017/?retryWrites=true&w=majority";
        var client = new MongoClient(connectionString);

        _bucket = new GridFSBucket(client.GetDatabase("game-news-media"), new GridFSBucketOptions
        {
            BucketName = "game-news-media"
        });
    }

    public async Task Save(
        Guid articleId,
        Guid mediaId,
        string type,
        byte[] source,
        CancellationToken cancellationToken)
    {
        var options = new GridFSUploadOptions
        {
            Metadata = new BsonDocument
            {
                { "articleId", articleId },
                { "mediaId", mediaId },
                { "type", type },
                { "alt", "" }
            }
        };

        var filename = $"{articleId}.{mediaId}";
        await _bucket.UploadFromBytesAsync(filename, source, options, cancellationToken);
    }

    public async Task<Result<MediaModel>> Get(
        Guid articleId,
        Guid mediaId,
        CancellationToken cancellationToken)
    {
        var fileInfo = await Find(articleId, mediaId, cancellationToken);
        if (fileInfo is null)
        {
            return Result.Fail(new MediaNotFoundError());
        }

        var source = await _bucket.DownloadAsBytesAsync(fileInfo.Id, cancellationToken: cancellationToken);

        return new MediaModel(
            articleId,
            mediaId,
            fileInfo.Metadata["type"].AsString,
            source
        );
    }

    public async Task<Result<MetaMediaModel>> GetMeta(Guid articleId, Guid mediaId, CancellationToken cancellationToken)
    {
        var fileInfo = await Find(articleId, mediaId, cancellationToken);
        if (fileInfo is null)
        {
            return Result.Fail(new MediaNotFoundError());
        }

        return new MetaMediaModel(
            articleId,
            mediaId,
            fileInfo.Metadata["type"].AsString,
            fileInfo.Metadata["alt"].AsString
        );
    }

    public async Task<IEnumerable<MetaMediaModel>> GetAllMetaByArticle(
        Guid articleId,
        CancellationToken cancellationToken)
    {
        var filesInfo = await FindByArticle(articleId, cancellationToken);

        return filesInfo.Select(f => new MetaMediaModel(
            f.Metadata["articleId"].AsGuid,
            f.Metadata["mediaId"].AsGuid,
            f.Metadata["type"].AsString,
            f.Metadata["alt"].AsString
        ));
    }

    public async Task<Result> UpdateMeta(
        MetaMediaModel metaMedia,
        CancellationToken cancellationToken)
    {
        var (articleId, mediaId, type, alt) = metaMedia;
        var fileInfo = await Find(articleId, mediaId, cancellationToken);
        if (await Find(articleId, mediaId, cancellationToken) is null)
        {
            return Result.Fail(new MediaNotFoundError());
        }

        var source = await _bucket.DownloadAsBytesAsync(fileInfo.Id, cancellationToken: cancellationToken);

        var options = new GridFSUploadOptions
        {
            Metadata = new BsonDocument
            {
                { "articleId", articleId },
                { "mediaId", mediaId },
                { "type", fileInfo.Metadata["type"].AsString },
                { "alt", alt ?? fileInfo.Metadata["alt"].AsString }
            }
        };
        var filename = $"{articleId}.{mediaId}";
        await _bucket.UploadFromBytesAsync(filename, source, options, cancellationToken);

        return Result.Ok();
    }


    public async Task<Result> Delete(Guid articleId, Guid mediaId, CancellationToken cancellationToken)
    {
        var fileInfo = await Find(articleId, mediaId, cancellationToken);
        if (fileInfo is null)
        {
            return Result.Fail(new MediaNotFoundError());
        }

        await _bucket.DeleteAsync(fileInfo.Id, cancellationToken);

        return Result.Ok();
    }

    public async Task DeleteAllByArticle(Guid articleId, CancellationToken cancellationToken)
    {
        foreach (var fileInfo in await FindByArticle(articleId, cancellationToken))
        {
            await _bucket.DeleteAsync(fileInfo.Id, cancellationToken);
        }
    }

    private async Task<GridFSFileInfo?> Find(Guid articleId, Guid mediaId, CancellationToken cancellationToken)
    {
        var filter = Builders<GridFSFileInfo>.Filter.And(
            Builders<GridFSFileInfo>.Filter.Eq(f => f.Metadata["articleId"], articleId),
            Builders<GridFSFileInfo>.Filter.Eq(f => f.Metadata["mediaId"], mediaId)
        );

        using var cursor = await _bucket.FindAsync(filter, cancellationToken: cancellationToken);
        return cursor.FirstOrDefault(cancellationToken);
    }

    private async Task<IEnumerable<GridFSFileInfo>> FindByArticle(Guid articleId, CancellationToken cancellationToken)
    {
        var filter = Builders<GridFSFileInfo>.Filter
            .Eq(f => f.Metadata["articleId"], articleId);

        using var cursor = await _bucket.FindAsync(filter, cancellationToken: cancellationToken);
        return await cursor.ToListAsync(cancellationToken);
    }
}