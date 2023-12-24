using FluentResults;
using GameNews.Articles.Application.Interfaces;
using GameNews.Articles.Application.Shared;
using GameNews.Articles.Domain.Errors;
using GameNews.Articles.Domain.Models;
using MediatR;

namespace GameNews.Articles.Application.Commands;

public record UpdateArticleCommand(
    Guid ArticleId,
    string? NewTitle,
    string? NewContent,
    List<Guid>? TagIds,
    User User
) : IRequest<Result>;

internal sealed class UpdateArticleCommandHandler(
    IArticleRepository articleRepository
) : IRequestHandler<UpdateArticleCommand, Result>
{
    public async Task<Result> Handle(UpdateArticleCommand request, CancellationToken cancellationToken)
    {
        var (articleId, newTitle, newContent, tagIds, user) = request;

        var oldArticle = await articleRepository.GetArticleById(articleId, cancellationToken);
        if (oldArticle is null)
        {
            return Result.Fail(new ArticleNotFoundError());
        }

        if (!(user.Role is User.AdministratorRole
              || user.Role is User.AuthorRole &&
              string.Compare(user.Id, oldArticle.AuthorId, StringComparison.Ordinal) == 0))
        {
            return Result.Fail(new AccessDeniedError());
        }

        var tags = await articleRepository.GetTagsByIds(tagIds ?? [], cancellationToken);

        var createResult = ArticleModel.Create(
            articleId,
            newTitle ?? oldArticle.Title,
            oldArticle.PreviewMediaId,
            oldArticle.PreviewText,
            tagIds is null ? oldArticle.Tags : tags.ToList(),
            oldArticle.CreationDate,
            oldArticle.AuthorId,
            oldArticle.IsVisible,
            newContent ?? oldArticle.Content
        );
        if (createResult.IsFailed)
        {
            return Result.Fail(createResult.Errors.FirstOrDefault());
        }

        var article = createResult.Value;
        await articleRepository.UpdateArticle(article, cancellationToken);

        return Result.Ok();
    }
}