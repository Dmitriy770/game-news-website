namespace GameNews.Articles.Api.Responses;

public record GetTagResponse(
    Guid Id,
    string Name,
    string Description
);