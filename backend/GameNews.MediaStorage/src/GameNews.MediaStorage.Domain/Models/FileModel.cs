namespace GameNews.MediaStorage.Domain.Models;

public record FileModel(
    Guid Id,
    string ContentType,
    byte[] Bytes
    );