namespace GameNews.OAuth.Infrastructure.Options;

public record DiscordApiOptions
{
    public string ClientId { get; init; } = string.Empty;
    public string ClientSecret { get; init; } = string.Empty;
}