namespace GameNews.Articles.Api.Responses;

public record Tag(
    Guid Id,
    string Name,
    string Description
);