using GameNews.Articles.Application.Interfaces;
using GameNews.Articles.Application.Shared;
using GameNews.Articles.Domain.Models;
using MediatR;

namespace GameNews.Articles.Application.Queries.Articles;

public record GetPreviewsQuery(
    int Skip,
    int Take,
    bool IsVisible,
    string? Query,
    User User
) : IRequest<GetPreviewsResult>;

public record GetPreviewsResult(
    List<ArticlePreview> Previews
);

internal sealed class GetPreviewsQueryHandler(
    IArticleRepository articleRepository
) : IRequestHandler<GetPreviewsQuery, GetPreviewsResult>
{
    public async Task<GetPreviewsResult> Handle(GetPreviewsQuery request, CancellationToken cancellationToken)
    {
        var (skip, take, isVisible, query, user) = request;

        IEnumerable<ArticleModel> articles = [];

        if (isVisible)
        {
            articles = await articleRepository.GetShownArticles(query, skip, take, cancellationToken);
        }
        else
        {
            articles = user.Role switch
            {
                User.AdministratorRole => await articleRepository.GetHiddenArticles(query, skip, take,
                    cancellationToken),
                User.AuthorRole => await articleRepository.GetHiddenArticlesByAuthor(user.Id, query, skip, take,
                    cancellationToken),
                _ => articles
            };
        }

        return new GetPreviewsResult(
            articles.Select(a => new ArticlePreview(
                a.Id,
                a.Title,
                a.PreviewMediaId,
                a.PreviewText,
                a.Tags.Select(t => new Tag(t.Id, t.Name, t.Description)).ToList(),
                new ArticleMeta(
                    a.CreationDate,
                    new Author(a.AuthorId)
                )
            )).ToList()
        );
    }
}