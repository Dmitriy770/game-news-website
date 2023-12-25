namespace GameNews.Articles.Api.Requests.Tags;

public record UpdateTagRequest(
    string Name,
    string Description
);