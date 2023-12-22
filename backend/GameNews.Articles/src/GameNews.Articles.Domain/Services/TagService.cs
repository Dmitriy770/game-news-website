using FluentResults;
using GameNews.Articles.Domain.DTOs;
using GameNews.Articles.Domain.Errors;
using GameNews.Articles.Domain.Interfaces;
using GameNews.Articles.Domain.Models;
using GameNews.Articles.Domain.Models.ValueTypes;
using GameNews.Articles.Domain.Services.Interfaces;

namespace GameNews.Articles.Domain.Services;

public sealed class TagService(
    IArticleRepository articleRepository
) : ITagService
{
    public async Task<Result<Guid>> Save(TagModel tag, UserModel user, CancellationToken cancellationToken)
    {
        if (user.Role.Value is not (RoleType.Author or RoleType.Administrator))
        {
            return Result.Fail(new AccessDeniedError());
        }

        await articleRepository.SaveTag(tag, cancellationToken);

        return tag.Id;
    }

    public async Task<Result> Update(UpdateTagDto tagDto, UserModel user, CancellationToken cancellationToken)
    {
        if (user.Role.Value is not (RoleType.Author or RoleType.Administrator))
        {
            return Result.Fail(new AccessDeniedError());
        }

        var result = await articleRepository.GetTagById(tagDto.Id, cancellationToken);
        if (result.HasError<TagNotFoundError>())
        {
            return Result.Fail(new TagNotFoundError());
        }

        var oldTag = result.Value;

        var newTagResult = TagModel.Create(
            tagDto.Id,
            tagDto.Name ?? oldTag.Name,
            tagDto.Description ?? oldTag.Description
        );
        if (newTagResult.HasError<ValidateError>())
        {
            return Result.Fail(new ValidateError("Tag validate error"));
        }

        await articleRepository.SaveTag(newTagResult.Value, cancellationToken);

        return Result.Ok();
    }

    public async Task<Result> Delete(Guid id, UserModel user, CancellationToken cancellationToken)
    {
        if (user.Role.Value is not (RoleType.Author or RoleType.Administrator))
        {
            return Result.Fail(new AccessDeniedError());
        }

        await articleRepository.DeleteTag(id, cancellationToken);

        return Result.Ok();
    }

    public Task<Result<TagModel>> GetById(Guid id, CancellationToken cancellationToken)
        => articleRepository.GetTagById(id, cancellationToken);

    public IAsyncEnumerable<TagModel> GetAll(CancellationToken cancellationToken)
        => articleRepository.GetAllTags(cancellationToken);
}