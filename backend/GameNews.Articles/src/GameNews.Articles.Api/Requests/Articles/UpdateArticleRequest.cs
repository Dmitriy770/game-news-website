namespace GameNews.Articles.Api.Requests.Articles;

public record UpdateArticleRequest(
    string? Title,
    List<Guid>? Tags,
    string? Content
);