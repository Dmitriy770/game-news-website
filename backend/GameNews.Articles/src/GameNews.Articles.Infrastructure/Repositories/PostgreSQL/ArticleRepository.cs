using FluentResults;
using GameNews.Articles.Application.Interfaces;
using GameNews.Articles.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace GameNews.Articles.Infrastructure.Repositories.PostgreSQL;

public sealed class ArticleRepository(
    ArticleRepositoryContext context
) : IArticleRepository
{
    public async Task AddArticle(ArticleModel article, CancellationToken cancellationToken)
    {
        await context.Articles.AddAsync(article, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateArticle(ArticleModel article, CancellationToken cancellationToken)
    {
        await context.Articles.Where(a => a.Id == article.Id).ExecuteUpdateAsync(
            s => s
                .SetProperty(a => a.Title, article.Title)
                .SetProperty(a => a.PreviewMediaId, article.PreviewMediaId)
                .SetProperty(a => a.PreviewText, article.PreviewText)
                .SetProperty(a => a.CreationDate, article.CreationDate)
                .SetProperty(a => a.AuthorId, article.AuthorId)
                .SetProperty(a => a.IsVisible, article.IsVisible)
                .SetProperty(a => a.Content, article.Content),
            cancellationToken
        );

        var dbArticle = (await context.Articles.FindAsync(article.Id, cancellationToken))!;
        await context.Entry(dbArticle).Collection("Tags").LoadAsync(cancellationToken);
        dbArticle.Tags.Clear();
        dbArticle.Tags.AddRange(article.Tags);

        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteArticle(Guid articleId, CancellationToken cancellationToken)
    {
        await context.Articles.Where(a => a.Id == articleId).ExecuteDeleteAsync(cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<ArticleModel?> GetArticleById(Guid articleId, CancellationToken cancellationToken)
    {
        return await context.Articles
            .Where(a => a.Id == articleId)
            .Include(a => a.Tags)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<ArticleModel>> GetHiddenArticlesByAuthor(
        string authorId,
        string? query,
        int skip,
        int take,
        CancellationToken cancellationToken)
    {
        return await context.Articles
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
        return await context.Articles
            .Where(a => !a.IsVisible)
            .Where(a => query == null || a.Title.Contains(query))
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ArticleModel>> GetShownArticles(string? query, int skip, int take,
        CancellationToken cancellationToken)
    {
        return await context.Articles
            .Where(a => a.IsVisible)
            .Where(a => query == null || a.Title.Contains(query))
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public async Task AddTag(TagModel tag, CancellationToken cancellationToken)
    {
        await context.Tags.AddAsync(tag, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateTag(TagModel tag, CancellationToken cancellationToken)
    {
        await context.Tags.Where(t => t.Id == tag.Id).ExecuteUpdateAsync(
            s => s
                .SetProperty(t => t.Name, tag.Name)
                .SetProperty(t => t.Description, tag.Description),
            cancellationToken
        );

        await context.SaveChangesAsync(cancellationToken);
    }

    public Task<TagModel?> GetTagById(Guid tagId, CancellationToken cancellationToken)
    {
        return context.Tags
            .Where(t => t.Id == tagId)
            .Include(t => t.Articles)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<TagModel>> GetTagsByIds(List<Guid> tagIds, CancellationToken cancellationToken)
    {
        return await context.Tags.Where(t => tagIds.Contains(t.Id))
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<TagModel>> GetAllTags(CancellationToken cancellationToken)
    {
        return await context.Tags.ToListAsync(cancellationToken);
    }

    public async Task DeleteTag(Guid tagId, CancellationToken cancellationToken)
    {
        await context.Tags
            .Where(t => t.Id == tagId)
            .ExecuteDeleteAsync(cancellationToken);

        await context.SaveChangesAsync(cancellationToken);
    }
}