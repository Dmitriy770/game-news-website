namespace GameNews.Articles.Api.Requests;

public record UpdatePreviewArticleRequest(
    Guid? PreviewMediaId,
    string? PreviewText
);