using FluentResults;
using GameNews.Articles.Application.Interfaces;
using GameNews.Articles.Application.Shared;
using GameNews.Articles.Domain.Errors;
using MediatR;

namespace GameNews.Articles.Application.Commands.Articles;

public record DeleteArticleCommand(
    Guid ArticleId,
    User User
) : IRequest<Result>;

internal sealed class DeleteArticleCommandHandler(
    IArticleRepository articleRepository
) : IRequestHandler<DeleteArticleCommand, Result>
{
    public async Task<Result> Handle(DeleteArticleCommand request, CancellationToken cancellationToken)
    {
        var (articleId, user) = request;
        
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

        await articleRepository.DeleteArticle(articleId, cancellationToken);

        return Result.Ok();
    }
}