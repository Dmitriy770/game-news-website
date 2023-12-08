namespace GameNews.Articles.Domain.Models;

public record ArticleModel
{
    public Guid Id { get; init; }
    public DateTime CreationDate { get; init; }
    public string AuthorId { get; init; } = string.Empty;
    public bool IsVisible { get; init; }
    
    public string? Title { get; init; }
    public string? PreviewText { get; init; }
    public Guid? PreviewMediaId { get; init; }
    public List<TagModel> Tags { get; init; } = new();
};