namespace GameNews.Articles.Api.Requests;

public record UpdateArticleRequest(
    string? Title,
    List<Guid>? Tags,
    string? Content
);