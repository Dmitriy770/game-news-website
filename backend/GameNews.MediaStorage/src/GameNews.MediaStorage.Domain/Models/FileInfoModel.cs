namespace GameNews.MediaStorage.Domain.Models;

public record FileInfoModel(
    Guid ArticleId,
    Guid Id,
    string ContentType
);