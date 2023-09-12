namespace GameNews.Articles.Infrastructure.Storage.Entities;

public class DescriptionEntity
{
    public long ArticleId { get; init; }
    public string Title { get; init; } = string.Empty;
    public string PreviewImage { get; init; } = string.Empty; 
    public DateTime PublicationDate { get; init; }
}