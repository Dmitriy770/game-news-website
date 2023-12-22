using System.Security.Principal;
using FluentResults;
using GameNews.Articles.Domain.DTOs;
using GameNews.Articles.Domain.Errors;
using GameNews.Articles.Domain.Interfaces;
using GameNews.Articles.Domain.Models;
using GameNews.Articles.Domain.Models.ValueTypes;

namespace GameNews.Articles.Domain.Services;

public sealed class ArticleService(
    IArticleRepository articleRepository
)
{
    public async Task<Result<ArticleModel>> Create(
        ArticleModel article,
        UserModel user,
        CancellationToken cancellationToken)
    {
        if (user.Role.Value is not (RoleType.Author or RoleType.Administrator))
        {
            return Result.Fail(new AccessDeniedError());
        }

        await articleRepository.SaveArticle(article, cancellationToken);

        return Result.Ok();
    }

    public async Task<Result<ArticleModel>> Update(
        UpdateArticleDto updateArticleDto,
        UserModel user,
        CancellationToken cancellationToken
    )
    {
        var getResult = await articleRepository.GetArticleById(updateArticleDto.Id, cancellationToken);
        if (getResult.IsFailed)
        {
            return Result.Fail(new ArticleNotFoundError());
        }

        var oldArticle = getResult.Value;
        if (!(user.Role.Value is RoleType.Administrator ||
              user.Role.Value is RoleType.Author && user.Id.CompareTo(oldArticle.AuthorId) == 0))
        {
            return Result.Fail(new AccessDeniedError());
        }

        var createResult = ArticleModel.Create(
            oldArticle.Id,
            updateArticleDto.Title ?? oldArticle.Title,
            updateArticleDto.PreviewMediaId ?? oldArticle.PreviewMediaId,
            updateArticleDto.PreviewText ?? oldArticle.PreviewText,
            updateArticleDto.Tags ?? oldArticle.Tags,
            oldArticle.CreationDate,
            oldArticle.AuthorId,
            oldArticle.IsVisible,
            updateArticleDto.Content ?? oldArticle.Content
        );
        if (createResult.IsFailed)
        {
            return Result.Fail(new ValidateError("error update article"));
        }

        await articleRepository.SaveArticle(createResult.Value, cancellationToken);

        return Result.Ok();
    }

    public async Task<Result> Delete(Guid id, UserModel user, CancellationToken cancellationToken)
    {
        var getResult = await articleRepository.GetArticleById(id, cancellationToken);
        if (getResult.IsFailed)
        {
            return Result.Fail(new ArticleNotFoundError());
        }

        if (!(user.Role.Value is RoleType.Administrator ||
              user.Role.Value is RoleType.Author && user.Id.CompareTo(getResult.Value.AuthorId) == 0))
        {
            return Result.Fail(new AccessDeniedError());
        }

        await articleRepository.DeleteArticle(id, cancellationToken);

        return Result.Ok();
    }

    public async Task<Result<ArticleModel>> GetArticle(Guid id, CancellationToken cancellationToken)
    {
        var result = await articleRepository.GetArticleById(id, cancellationToken);
        if (result.IsFailed)
        {
            return Result.Fail(new ArticleNotFoundError());
        }

        return result.Value;
    }
    
    
}