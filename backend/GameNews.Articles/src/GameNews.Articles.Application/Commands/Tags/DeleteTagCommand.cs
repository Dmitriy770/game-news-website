using FluentResults;
using GameNews.Articles.Application.Interfaces;
using GameNews.Articles.Application.Shared;
using GameNews.Articles.Domain.Errors;
using MediatR;

namespace GameNews.Articles.Application.Commands.Tags;

public record DeleteTagCommand(
    Guid TagId,
    User User
) : IRequest<Result>;

internal sealed class DeleteTagCommandHandler(
    IArticleRepository articleRepository
) : IRequestHandler<DeleteTagCommand, Result>
{
    public async Task<Result> Handle(DeleteTagCommand request, CancellationToken cancellationToken)
    {
        var (tagId, user) = request;
        
        if (user.Role is not (User.AdministratorRole or User.AuthorRole))
        {
            return Result.Fail(new AccessDeniedError());
        }

        var tag = await articleRepository.GetTagById(tagId, cancellationToken);
        if (tag is null)
        {
            return Result.Fail(new TagNotFoundError());
        }

        if (tag.Articles.Count > 0)
        {
            return Result.Fail(new DeleteTagError());
        }

        await articleRepository.DeleteTag(tagId, cancellationToken);

        return Result.Ok();
    }
}