namespace GameNews.Articles.Domain.Models;

public record TagModel
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
};