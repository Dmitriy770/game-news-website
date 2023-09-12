namespace GameNews.Articles.Domain.Models;

public record ArticleModel
{
    public long Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Content { get; init; } = string.Empty;
    public string Image { get; init; } = string.Empty;
    public DateTime PublicationDate { get; init; }
}