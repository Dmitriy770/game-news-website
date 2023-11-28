namespace GameNews.MediaStorage.Domain.Models;

public record FileModel(
    Guid ArticleId,
    Guid Id,
    string ContentType,
    byte[] Source
    );