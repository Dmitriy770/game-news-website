namespace GameNews.Articles.Api.Requests.Tags;

public record AddTagRequest(
    string Name,
    string Description
);