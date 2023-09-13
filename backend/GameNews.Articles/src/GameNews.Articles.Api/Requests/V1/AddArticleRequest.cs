namespace GameNews.Articles.Api.Requests.V1;

public record AddArticleRequest
{
    public string Title { get; init; } = string.Empty;
    public string Content { get; init; } = string.Empty;
    public string Image { get; init; } = string.Empty;
    public DateTime PublicationDate { get; init; }
}