using FluentResults;
using GameNews.Articles.Application.Interfaces;
using GameNews.Articles.Application.Shared;
using GameNews.Articles.Domain.Errors;
using MediatR;

namespace GameNews.Articles.Application.Commands.Articles;

public record ChangeVisibilityArticleCommand(
    Guid ArticleId,
    bool IsVisible,
    User User
) : IRequest<Result>;

internal sealed class ChangeVisibilityArticleCommandHandler(
    IArticleRepository articleRepository
) : IRequestHandler<ChangeVisibilityArticleCommand, Result>
{
    public async Task<Result> Handle(ChangeVisibilityArticleCommand request, CancellationToken cancellationToken)
    {
        var (articleId, isVisible, user) = request;

        var article = await articleRepository.GetArticleById(articleId, cancellationToken);
        if (article is null)
        {
            return Result.Fail(new ArticleNotFoundError());
        }

        if (!(user.Role is User.AdministratorRole
              || user.Role is User.AuthorRole &&
              string.Compare(user.Id, article.AuthorId, StringComparison.Ordinal) == 0))
        {
            return Result.Fail(new AccessDeniedError());
        }

        if (isVisible)
        {
            article.Show();
        }
        else
        {
            article.Hide();
        }

        await articleRepository.UpdateArticle(article, cancellationToken);

        return Result.Ok();
    }
}