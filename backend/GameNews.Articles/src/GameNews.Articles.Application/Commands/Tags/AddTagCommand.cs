using FluentResults;
using GameNews.Articles.Application.Interfaces;
using GameNews.Articles.Application.Shared;
using GameNews.Articles.Domain.Errors;
using GameNews.Articles.Domain.Models;
using MediatR;

namespace GameNews.Articles.Application.Commands.Tags;

public record AddTagCommand(
    Tag Tag,
    User User
) : IRequest<Result<Tag>>;

internal sealed class AddTagCommandHandler(
    IArticleRepository articleRepository
) : IRequestHandler<AddTagCommand, Result<Tag>>
{
    public async Task<Result<Tag>> Handle(AddTagCommand request, CancellationToken cancellationToken)
    {
        var (tag, user) = request;

        if (user.Role is not (User.AdministratorRole or User.AuthorRole))
        {
            return Result.Fail(new AccessDeniedError());
        }

        var createResult = TagModel.Create(tag.Id, tag.Name, tag.Description);
        if (createResult.IsFailed)
        {
            return Result.Fail(createResult.Errors.FirstOrDefault());
        }

        var tagModel = createResult.Value;
        await articleRepository.AddTag(tagModel, cancellationToken);

        return new Tag(tagModel.Id, tagModel.Name, tagModel.Description);
    }
}