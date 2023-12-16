namespace GameNews.Articles.Api.Requests;

public record UpdateTagRequest(
    string Name,
    string Description
);