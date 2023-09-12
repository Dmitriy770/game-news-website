namespace GameNews.Articles.Domain.Models;

public record DescriptionModel
{
    public long Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Image { get; init; } = string.Empty; 
    public DateTime PublicationDate { get; init; }
}