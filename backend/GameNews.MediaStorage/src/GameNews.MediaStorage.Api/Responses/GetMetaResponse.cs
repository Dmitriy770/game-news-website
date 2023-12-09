namespace GameNews.MediaStorage.Api.Responses;

public record GetMetaResponse(
    Guid ArticleId,
    Guid MediaId,
    string Type,
    string Alt
);