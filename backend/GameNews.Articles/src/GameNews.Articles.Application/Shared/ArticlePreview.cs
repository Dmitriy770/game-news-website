namespace GameNews.Articles.Application.Shared;

public record ArticlePreview(
    Guid Id,
    string Title,
    Guid? PreviewMediaId,
    string? PreviewText,
    List<Tag> Tags,
    ArticleMeta Meta
);