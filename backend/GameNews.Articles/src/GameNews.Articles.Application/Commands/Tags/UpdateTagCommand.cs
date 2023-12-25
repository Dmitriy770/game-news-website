using FluentResults;
using GameNews.Articles.Application.Interfaces;
using GameNews.Articles.Application.Shared;
using GameNews.Articles.Domain.Errors;
using GameNews.Articles.Domain.Models;
using MediatR;

namespace GameNews.Articles.Application.Commands.Tags;

public record UpdateTagCommand(
    Tag NewTag,
    User User
) : IRequest<Result>;

internal sealed class UpdateTagCommandHandler(
    IArticleRepository articleRepository
) : IRequestHandler<UpdateTagCommand, Result>
{
    public async Task<Result> Handle(UpdateTagCommand request, CancellationToken cancellationToken)
    {
        var (newTag, user) = request;

        if (user.Role is not (User.AdministratorRole or User.AuthorRole))
        {
            return Result.Fail(new AccessDeniedError());
        }

        var oldTag = await articleRepository.GetTagById(newTag.Id, cancellationToken);
        if (oldTag is null)
        {
            return Result.Fail(new TagNotFoundError());
        }
        
        var createResult = TagModel.Create(newTag.Id, newTag.Name, newTag.Description);
        if (createResult.IsFailed)
        {
            return Result.Fail(createResult.Errors.FirstOrDefault());
        }
        
        await articleRepository.UpdateTag(createResult.Value, cancellationToken);

        return Result.Ok();
    }
}