namespace GameNews.Articles.Api.Requests;

public record CreateTagRequest(
    string Name,
    string Description
);