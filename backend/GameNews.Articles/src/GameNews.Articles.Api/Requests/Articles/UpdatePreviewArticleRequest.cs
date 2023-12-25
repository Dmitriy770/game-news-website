namespace GameNews.Articles.Api.Requests.Articles;

public record UpdatePreviewArticleRequest(
    Guid? PreviewMediaId,
    string? PreviewText
);