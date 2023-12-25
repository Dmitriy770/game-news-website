using FluentResults;
using GameNews.Articles.Application.Interfaces;
using GameNews.Articles.Application.Shared;
using GameNews.Articles.Domain.Errors;
using GameNews.Articles.Domain.Models;
using MediatR;

namespace GameNews.Articles.Application.Commands.Articles;

public record UpdatePreviewArticleCommand(
    Guid ArticleId,
    Guid? NewPreviewMediaId,
    string? NewPreviewText,
    User User
) : IRequest<Result>;

internal sealed class UpdatePreviewArticleCommandHandler(
    IArticleRepository articleRepository
) : IRequestHandler<UpdatePreviewArticleCommand, Result>
{
    public async Task<Result> Handle(UpdatePreviewArticleCommand request, CancellationToken cancellationToken)
    {
        var (articleId, newPreviewMediaId, newPreviewText, user) = request;
        
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
        
        var createResult = ArticleModel.Create(
            articleId,
            oldArticle.Title,
            newPreviewMediaId ?? oldArticle.PreviewMediaId,
            newPreviewText ?? oldArticle.PreviewText,
            oldArticle.Tags,
            oldArticle.CreationDate,
            oldArticle.AuthorId,
            oldArticle.IsVisible,
            oldArticle.Content
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