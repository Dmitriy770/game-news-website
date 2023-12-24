using GameNews.Articles.Application.Interfaces;
using GameNews.Articles.Application.Shared;
using GameNews.Articles.Domain.Models;
using MediatR;

namespace GameNews.Articles.Application.Queries;

public record GetPreviewsQuery(
    int Skip,
    int Take,
    bool IsVisible,
    string? Query,
    User User
) : IRequest<IEnumerable<ArticlePreview>>;

internal sealed class GetPreviewsQueryHandler(
    IArticleRepository articleRepository
) : IRequestHandler<GetPreviewsQuery, IEnumerable<ArticlePreview>>
{
    public async Task<IEnumerable<ArticlePreview>> Handle(GetPreviewsQuery request, CancellationToken cancellationToken)
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

        return articles.Select(a => new ArticlePreview(
            a.Id,
            a.Title,
            a.PreviewMediaId,
            a.PreviewText,
            new ArticleMeta(a.CreationDate, a.AuthorId)
        ));
    }
}