namespace GameNews.MediaStorage.Domain.Dto;

public record SaveMediaDto(
    Guid ArticleId,
    Guid MediaId,
    string Type,
    byte[] Source
);