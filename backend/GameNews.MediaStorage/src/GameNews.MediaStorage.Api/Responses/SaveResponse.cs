namespace GameNews.MediaStorage.Api.Responses;

public record SaveResponse(
    Guid ArticleId,
    Guid MediaId
);