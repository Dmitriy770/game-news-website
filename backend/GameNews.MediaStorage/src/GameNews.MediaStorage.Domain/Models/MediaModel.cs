namespace GameNews.MediaStorage.Domain.Models;

public record MediaModel(
    Guid ArticleId,
    Guid Id,
    string ContentType,
    byte[] Source
    );