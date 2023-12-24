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
) : IArticleRepository, Application.Interfaces.IArticleRepository
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

    public async Task<IEnumerable<TagModel>> GetTagsByIds(List<Guid> tagIds, CancellationToken cancellationToken)
    {
        var tagEntities = await dbContext.Tags.Where(t => tagIds.Contains(t.Id)).ToListAsync(cancellationToken);
        return tagEntities.Select(t => TagModel.Create(t.Id, t.Name, t.Description).Value).ToList();
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

    public async Task AddArticle(ArticleModel article, CancellationToken cancellationToken)
    {
        await dbContext.Articles.AddAsync(article, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateArticle(ArticleModel article, CancellationToken cancellationToken)
    {
        await dbContext.Articles.Where(a => a.Id == article.Id).ExecuteUpdateAsync(
            s => s
                .SetProperty(a => a.Title, article.Title)
                .SetProperty(a => a.PreviewMediaId, article.PreviewMediaId)
                .SetProperty(a => a.PreviewText, article.PreviewText)
                // .SetProperty(a => a.Tags, article.Tags)
                .SetProperty(a => a.CreationDate, article.CreationDate)
                .SetProperty(a => a.AuthorId, article.AuthorId)
                .SetProperty(a => a.IsVisible, article.IsVisible)
                .SetProperty(a => a.Content, article.Content),
            cancellationToken
        );
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteArticle(Guid articleId, CancellationToken cancellationToken)
    {
        await dbContext.Articles.Where(a => a.Id == articleId).ExecuteDeleteAsync(cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<ArticleModel?> GetArticleById(Guid articleId, CancellationToken cancellationToken)
    {
        return await dbContext.Articles.FindAsync(articleId, cancellationToken);
    }

    public async Task<IEnumerable<ArticleModel>> GetHiddenArticlesByAuthor(
        string authorId,
        string? query,
        int skip,
        int take,
        CancellationToken cancellationToken)
    {
        return await dbContext.Articles
            .Where(a => !a.IsVisible)
            .Where(a => a.AuthorId == authorId)
            .Where(a => query == null || a.Title.Contains(query))
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ArticleModel>> GetHiddenArticles(string? query, int skip, int take,
        CancellationToken cancellationToken)
    {
        return await dbContext.Articles
            .Where(a => !a.IsVisible)
            .Where(a => query == null || a.Title.Contains(query))
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ArticleModel>> GetShownArticles(string? query, int skip, int take,
        CancellationToken cancellationToken)
    {
        return await dbContext.Articles
            .Where(a => a.IsVisible)
            .Where(a => query == null || a.Title.Contains(query))
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);
    }
}