using GameNews.Articles.Domain.Errors;
using GameNews.Articles.Domain.Interfaces;
using GameNews.Articles.Domain.Models;
using OneOf;

namespace GameNews.Articles.Domain.Services;

public sealed class ArticleService
{
    private readonly IArticleRepository _articleRepository;

    public ArticleService(IArticleRepository articleRepository)
    {
        _articleRepository = articleRepository;
    }

    public async Task<OneOf<ArticleModel, AccessDeniedError>> Create(
        UserModel user,
        CancellationToken cancellationToken)
    {
        if (user.Role is RoleModel.Anonymous or RoleModel.User)
        {
            return new AccessDeniedError();
        }

        var article = new ArticleModel
        {
            Id = new Guid(),
            CreationDate = DateTime.Now,
            AuthorId = user.Id
        };

        await _articleRepository.SaveArticle(article, cancellationToken);

        return article;
    }

    public async Task<OneOf<ArticleModel, ArticleNotFoundError, AccessDeniedError>> Update(
        ArticleModel article,
        CancellationToken cancellationToken)
    {
        if ((await _articleRepository.GetArticle(article.Id, cancellationToken)).Value is ArticleNotFoundError)
        {
            return new ArticleNotFoundError();
        }

        return new ArticleModel();
    }
}