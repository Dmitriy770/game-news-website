using System.Collections;
using GameNews.Articles.Domain.Models;

namespace GameNews.Articles.Application.Interfaces;

public interface IArticleRepository
{ 
    Task AddArticle(ArticleModel article, CancellationToken cancellationToken);
    Task UpdateArticle(ArticleModel article, CancellationToken cancellationToken);

    Task DeleteArticle(Guid articleId, CancellationToken cancellationToken);
    
    Task<ArticleModel?> GetArticleById(Guid articleId, CancellationToken cancellationToken);

    Task<IEnumerable<TagModel>> GetTagsByIds(List<Guid> tagIds, CancellationToken cancellationToken);

    Task<IEnumerable<ArticleModel>> GetHiddenArticlesByAuthor(
        string authorId,
        string? query,
        int skip,
        int take,
        CancellationToken cancellationToken);
    
    Task<IEnumerable<ArticleModel>> GetHiddenArticles(
        string? query,
        int skip,
        int take,
        CancellationToken cancellationToken);
    
    Task<IEnumerable<ArticleModel>> GetShownArticles(
        string? query,
        int skip,
        int take,
        CancellationToken cancellationToken);
    
    public Task AddTag(TagModel tag, CancellationToken cancellationToken);
    
    public Task UpdateTag(TagModel tag, CancellationToken cancellationToken);
    
    public Task<TagModel?> GetTagById(Guid tagId, CancellationToken cancellationToken);
    
    public Task<IEnumerable<TagModel>> GetAllTags(CancellationToken cancellationToken);

    public Task DeleteTag(Guid tagId, CancellationToken cancellationToken);
}