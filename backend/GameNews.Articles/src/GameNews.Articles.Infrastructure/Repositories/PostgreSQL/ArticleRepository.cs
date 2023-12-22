using FluentResults;
using GameNews.Articles.Domain.Errors;
using GameNews.Articles.Domain.Interfaces;
using GameNews.Articles.Domain.Models;
using GameNews.Articles.Infrastructure.Exceptions;
using GameNews.Articles.Infrastructure.Repositories.PostgreSQL.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameNews.Articles.Infrastructure.Repositories.PostgreSQL;

public sealed class ArticleRepository(
    ArticleRepositoryContext dbContext
) : IArticleRepository
{
    public async Task<Result> SaveTag(TagModel tag, CancellationToken cancellationToken)
    {
        if (await dbContext.Tags.FindAsync(tag.Id, cancellationToken) is null)
        {
            await dbContext.Tags.AddAsync(new TagEntity { Id = tag.Id, Name = tag.Name, Description = tag.Description },
                cancellationToken);
        }
        else
        {
            await dbContext.Tags.Where(t => t.Id == tag.Id)
                .ExecuteUpdateAsync(s => s
                        .SetProperty(t => t.Name, tag.Name)
                        .SetProperty(t => t.Description, tag.Description),
                    cancellationToken);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        return Result.Ok();
    }

    public async Task<Result> DeleteTag(Guid tagId, CancellationToken cancellationToken)
    {
        await dbContext.Tags.Where(t => t.Id == tagId).ExecuteDeleteAsync(cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return Result.Ok();
    }

    public async Task<Result<TagModel>> GetTagById(Guid tagId, CancellationToken cancellationToken)
    {
        var tagEntity = await dbContext.Tags.FindAsync(tagId, cancellationToken);

        if (tagEntity is null)
        {
            return Result.Fail(new TagNotFoundError());
        }

        var result = TagModel.Create(tagEntity.Id, tagEntity.Name, tagEntity.Description);
        if (result.IsFailed)
        {
            throw new InvalidDataInDbException();
        }

        return result.Value;
    }

    public async IAsyncEnumerable<TagModel> GetAllTags(CancellationToken cancellationToken)
    {
        await foreach (var tag in dbContext.Tags)
        {
            var result = TagModel.Create(tag.Id, tag.Name, tag.Description);
            if (result.IsFailed)
            {
                throw new InvalidDataInDbException();
            }

            yield return result.Value;
        }
    }

    public async Task<Result> SaveArticle(ArticleModel article, CancellationToken cancellationToken)
    {
        if (await dbContext.Articles.FindAsync(article.Id, cancellationToken) is null)
        {
            await dbContext.Articles.AddAsync(new ArticleEntity
                {
                    Id = article.Id,
                    Title = article.Title,
                    PreviewText = article.PreviewText,
                    PreviewMediaId = article.PreviewMediaId,
                    Tags = article.Tags.Select(t => new TagEntity
                    {
                        Id = t.Id,
                        Name = t.Name,
                        Description = t.Description
                    }).ToList(),
                    AuthorId = article.AuthorId,
                    CreationDate = article.CreationDate,
                    IsVisible = article.IsVisible,
                    Content = article.Content
                },
                cancellationToken);
        }
        else
        {
            await dbContext.Articles.Where(a => a.Id == article.Id)
                .ExecuteUpdateAsync(s => s
                        .SetProperty(a => a.Title, article.Title)
                        .SetProperty(a => a.PreviewText, article.PreviewText)
                        .SetProperty(a => a.PreviewMediaId, article.PreviewMediaId)
                        .SetProperty(a => a.Tags, article.Tags.Select(t => new TagEntity
                        {
                            Id = t.Id,
                            Name = t.Name,
                            Description = t.Description
                        }).ToList())
                        .SetProperty(a => a.IsVisible, article.IsVisible)
                        .SetProperty(a => a.Content, article.Content),
                    cancellationToken);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        return Result.Ok();
    }

    public async Task<Result<ArticleModel>> GetArticleById(Guid articleId, CancellationToken cancellationToken)
    {
        var articleEntity = await dbContext.Articles.FindAsync(articleId, cancellationToken);

        if (articleEntity is null)
        {
            return Result.Fail(new ArticleNotFoundError());
        }

        var result = ArticleModel.Create(
            articleEntity.Id,
            articleEntity.Title,
            articleEntity.PreviewMediaId,
            articleEntity.PreviewText,
            articleEntity.Tags.Select(t => TagModel.Create(t.Id, t.Name, t.Description).Value).ToList(),
            articleEntity.CreationDate,
            articleEntity.AuthorId,
            articleEntity.IsVisible,
            articleEntity.Content
        );
        if (result.IsFailed)
        {
            throw new InvalidDataInDbException();
        }

        return result.Value;
    }

    public async Task<Result> DeleteArticle(Guid articleId, CancellationToken cancellationToken)
    {
        await dbContext.Articles.Where(a => a.Id == articleId).ExecuteDeleteAsync(cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return Result.Ok();
    }
}