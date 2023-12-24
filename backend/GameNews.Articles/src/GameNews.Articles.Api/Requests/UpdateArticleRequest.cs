namespace GameNews.Articles.Api.Requests;

public record UpdateArticleRequest(
    string? Title,
    string? PreviewText,
    Guid? PreviewMediaId,
    List<Guid>? Tags,
    string? Content
);