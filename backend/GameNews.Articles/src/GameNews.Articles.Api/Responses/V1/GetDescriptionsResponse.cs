namespace GameNews.Articles.Api.Responses.V1;

public record GetDescriptionsResponse
{
    public IEnumerable<Description> Descriptions { get; init; }
}