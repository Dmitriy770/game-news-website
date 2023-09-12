using Dapper;
using GameNews.Articles.Domain.Interfaces;
using GameNews.Articles.Domain.Models;
using GameNews.Articles.Infrastructure.Storage.Entities;
using Microsoft.Extensions.Options;

namespace GameNews.Articles.Infrastructure.Storage.Repositories;

public class ArticleRepository : BaseDbRepository, IArticleRepository
{
    public ArticleRepository(IOptions<StorageOptions> dbSettings) : base(dbSettings.Value)
    {
    }

    public async Task<ArticleModel> Get(long id, CancellationToken token)
    {
        const string sqlQuery = @"
        SELECT article_id, title, preview_image, publication_date, content
        FROM articles
        WHERE article_id=@Id
        ";
        var sqlQueryParams = new
        {
            Id = id
        };

        await using var connection = await GetAndOpenConnection();
        var articles = await connection.QueryAsync<ArticleEntity>(
            new CommandDefinition(sqlQuery, sqlQueryParams, cancellationToken: token)
        );

        //TODO Ошибка, когда нет статьи
        
        var article = articles.First();
        return new ArticleModel
        {
            Id = article.ArticleId,
            Title = article.Title,
            Image = article.PreviewImage,
            Content = article.Content,
            PublicationDate = article.PublicationDate,
        };
    }

    public async Task<IEnumerable<DescriptionModel>> GetDescriptions(int take, int skip, CancellationToken token)
    {
        const string sqlQuery = @"
        SELECT article_id, title, preview_image, publication_date
        FROM articles
        ORDER BY publication_date DESC 
        LIMIT @Limit OFFSET @Offset;
        ";
        var sqlQueryParams = new
        {
            Limit = take,
            Offset = skip
        };

        await using var connection = await GetAndOpenConnection();
        var descriptions = await connection.QueryAsync<DescriptionEntity>(
            new CommandDefinition(sqlQuery, sqlQueryParams, cancellationToken: token)
        );

        return descriptions.Select(d => new DescriptionModel
        {
            Id = d.ArticleId,
            Title = d.Title,
            PublicationDate = d.PublicationDate,
            Image = d.PreviewImage
        });
    }

    public async Task<long> Add(ArticleModel article, CancellationToken token)
    {
        const string sqlQuery = @"
        INSERT INTO articles(title, preview_image, publication_date, content)
        VALUES (@Title, @PreviewImage, @PublicationDate, @Content)
        RETURNING article_id";
        var sqlQueryParams = new
        {
            Title = article.Title,
            PreviewImage = article.Image,
            PublicationDate = article.PublicationDate,
            Content = article.Content
        };

        await using var connection = await GetAndOpenConnection();
        var id = await connection.QueryAsync<long>(
            new CommandDefinition(sqlQuery, sqlQueryParams, cancellationToken: token)
        );

        return id.First();
    }

    public async Task Delete(long id, CancellationToken token)
    {
        const string sqlExec = @"
        DELETE FROM articles
        WHERE article_id=@Id;
        ";
        var sqlExecParams = new
        {
            Id = id
        };

        await using var connection = await GetAndOpenConnection();
        await connection.ExecuteAsync(
            new CommandDefinition(sqlExec, sqlExecParams, cancellationToken: token)
        );
    }
}