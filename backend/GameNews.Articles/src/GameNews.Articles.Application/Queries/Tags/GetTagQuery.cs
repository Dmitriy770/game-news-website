using FluentResults;
using GameNews.Articles.Application.Interfaces;
using GameNews.Articles.Application.Shared;
using GameNews.Articles.Domain.Errors;
using MediatR;

namespace GameNews.Articles.Application.Queries.Tags;

public record GetTagQuery(
    Guid TagId
) : IRequest<Result<Tag>>;

internal sealed class GetTagQueryHandler(
    IArticleRepository articleRepository
) : IRequestHandler<GetTagQuery, Result<Tag>>
{
    public async Task<Result<Tag>> Handle(GetTagQuery request, CancellationToken cancellationToken)
    {
        var tag = await articleRepository.GetTagById(request.TagId, cancellationToken);
        if (tag is null)
        {
            return Result.Fail(new TagNotFoundError());
        }

        return new Tag(tag.Id, tag.Name, tag.Description);
    }
}