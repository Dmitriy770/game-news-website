namespace GameNews.Articles.Api.Responses;

public record GetArticleResponse(
    Guid Id,
    string Title,
    DateTime CreationDate,
    string AuthorId,
    bool IsVisible,
    string? PreviewText,
    Guid? PreviewMediaId,
    List<Tag> Tags,
    string Content
);