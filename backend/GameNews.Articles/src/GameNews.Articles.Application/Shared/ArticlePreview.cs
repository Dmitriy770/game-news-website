namespace GameNews.Articles.Application.Shared;

public record ArticlePreview(
    Guid Id,
    string Title,
    Guid? PreviewMediaId,
    string? PreviewText,
    ArticleMeta Meta
);