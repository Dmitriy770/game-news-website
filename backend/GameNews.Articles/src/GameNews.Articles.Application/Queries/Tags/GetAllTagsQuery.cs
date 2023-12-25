using GameNews.Articles.Application.Interfaces;
using GameNews.Articles.Application.Shared;
using MediatR;

namespace GameNews.Articles.Application.Queries.Tags;

public record GetAllTagsQuery() : IRequest<GetAllTagsResult>;

public record GetAllTagsResult(
    List<Tag> Tags
);

internal sealed class GetAllTagsQueryHandler(
    IArticleRepository articleRepository
) : IRequestHandler<GetAllTagsQuery, GetAllTagsResult>
{
    public async Task<GetAllTagsResult> Handle(GetAllTagsQuery request, CancellationToken cancellationToken)
    {
        var tags = await articleRepository.GetAllTags(cancellationToken);

        return new GetAllTagsResult(
            tags.Select(t => new Tag(t.Id, t.Name, t.Description)).ToList()
        );
    }
}