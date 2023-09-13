namespace GameNews.Articles.Api.Responses.V1;

public record Description
{
    public long Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Image { get; init; } = string.Empty; 
    public DateTime PublicationDate { get; init; }
}