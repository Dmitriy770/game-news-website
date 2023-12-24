using FluentResults;
using GameNews.Articles.Application.Interfaces;
using GameNews.Articles.Application.Shared;
using GameNews.Articles.Domain.Errors;
using MediatR;

namespace GameNews.Articles.Application.Queries;

public record GetArticleQuery(
    Guid ArticleId,
    User User
) : IRequest<Result<GetArticleQueryResult>>;

public record GetArticleQueryResult(
    Guid Id,
    ArticlePreview Preview,
    string Content,
    List<Tag> Tags,
    ArticleMeta Meta
);

internal sealed class GetArticleQueryHandler(
    IArticleRepository articleRepository
) : IRequestHandler<GetArticleQuery, Result<GetArticleQueryResult>>
{
    public async Task<Result<GetArticleQueryResult>> Handle(GetArticleQuery request,
        CancellationToken cancellationToken)
    {
        var (articleId, user) = request;

        var article = await articleRepository.GetArticleById(articleId, cancellationToken);
        if (article is null)
        {
            return Result.Fail(new ArticleNotFoundError());
        }

        if (article.IsVisible is false &&
            !(user.Role is User.AdministratorRole
              || user.Role is User.AuthorRole &&
              string.Compare(user.Id, article.AuthorId, StringComparison.Ordinal) == 0))
        {
            return Result.Fail(new AccessDeniedError());
        }

        var articleMeta = new ArticleMeta(article.CreationDate, article.AuthorId);
        return new GetArticleQueryResult(
            article.Id,
            new ArticlePreview(article.Id, article.Title, article.PreviewMediaId, article.PreviewText, articleMeta),
            article.Content,
            // article.Tags.Select(t => new Tag(t.Id, t.Name, t.Description)).ToList(),
            [],
            articleMeta
        );
    }
}