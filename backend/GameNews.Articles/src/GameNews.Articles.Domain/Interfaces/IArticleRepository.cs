using FluentResults;
using GameNews.Articles.Domain.Models;

namespace GameNews.Articles.Domain.Interfaces;

public interface IArticleRepository
{
    public Task<Result> SaveTag(TagModel tag, CancellationToken cancellationToken);

    public Task<Result> DeleteTag(Guid tagId, CancellationToken cancellationToken);

    public Task<Result<TagModel>> GetTagById(Guid tagId, CancellationToken cancellationToken);

    public IAsyncEnumerable<TagModel> GetAllTags(CancellationToken cancellationToken);

    public Task<Result> SaveArticle(ArticleModel article, CancellationToken cancellationToken);

    public Task<Result<ArticleModel>> GetArticleById(Guid articleId, CancellationToken cancellationToken);

    public Task<Result> DeleteArticle(Guid articleId, CancellationToken cancellationToken);
}