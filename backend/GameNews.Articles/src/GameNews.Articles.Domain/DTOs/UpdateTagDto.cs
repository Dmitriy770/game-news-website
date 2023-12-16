namespace GameNews.Articles.Domain.DTOs;

public record UpdateTagDto(
    Guid Id,
    string? Name,
    string? Description
);