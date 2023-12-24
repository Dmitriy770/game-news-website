namespace GameNews.Articles.Api.Responses;

public record CreateArticleResponse(
    Guid Id,
    DateTime CreationDate,
    string AuthorId
);