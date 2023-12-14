using FluentResults;
using GameNews.Articles.Domain.Errors;
using GameNews.Articles.Domain.Interfaces;
using GameNews.Articles.Domain.Models;
using GameNews.Articles.Domain.Services.Interfaces;

namespace GameNews.Articles.Domain.Services;

public class TagService(
    IArticleRepository articleRepository
) : ITagService
{
    public async Task<Result> Save(TagModel tag, UserModel user, CancellationToken cancellationToken)
    {
        if (user.Role.Value is "Administrator")
        {
            return Result.Fail(new AccessDeniedError());
        }

        await articleRepository.SaveTag(tag, cancellationToken);

        return Result.Ok();
    }
}