using GameNews.Articles.Domain.Errors;
using GameNews.Articles.Domain.Models;
using OneOf;

namespace GameNews.Articles.Domain.Interfaces;

public interface IArticleRepository
{
    public Task SaveArticle(ArticleModel article, CancellationToken cancellationToken);

    public Task<OneOf<ArticleModel, ArticleNotFoundError>> GetArticle(Guid articleId, CancellationToken cancellationToken);
}