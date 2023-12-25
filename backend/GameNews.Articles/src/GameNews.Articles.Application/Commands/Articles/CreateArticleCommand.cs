using FluentResults;
using GameNews.Articles.Application.Interfaces;
using GameNews.Articles.Application.Shared;
using GameNews.Articles.Domain.Errors;
using GameNews.Articles.Domain.Models;
using MediatR;

namespace GameNews.Articles.Application.Commands.Articles;

public record CreateArticleCommand(
    Guid ArticleId,
    DateTime CreationDate,
    User User
) : IRequest<Result<CreateArticleResult>>;

public record CreateArticleResult(
    Guid Id,
    string Title,
    ArticleMeta Meta
);

internal sealed class CreateArticleCommandHandler(
    IArticleRepository articleRepository
) : IRequestHandler<CreateArticleCommand, Result<CreateArticleResult>>
{
    public async Task<Result<CreateArticleResult>> Handle(CreateArticleCommand request,
        CancellationToken cancellationToken)
    {
        var (articleId, creationDate, user) = request;

        if (user.Role is not (User.AdministratorRole or User.AuthorRole))
        {
            return Result.Fail(new AccessDeniedError());
        }

        var createResult = ArticleModel.Create(articleId, creationDate, user.Id);
        if (createResult.IsFailed)
        {
            return Result.Fail(createResult.Errors.FirstOrDefault());
        }

        var article = createResult.Value!;
        await articleRepository.AddArticle(article, cancellationToken);

        return new CreateArticleResult(
            article.Id,
            article.Title,
            new ArticleMeta(
                article.CreationDate,
                new Author(article.AuthorId)
            )
        );
    }
}