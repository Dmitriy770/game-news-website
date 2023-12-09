namespace GameNews.MediaStorage.Domain.Models;

public record MetaMediaModel(
    Guid ArticleId,
    Guid Id,
    string? Type,
    string? Alt
);