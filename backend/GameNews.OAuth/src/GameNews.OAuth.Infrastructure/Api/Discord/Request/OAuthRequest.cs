namespace GameNews.OAuth.Infrastructure.Api.Discord.Request;

public record OAuthRequest
{
    public string ClientId { get; init; } = string.Empty;
    public string ClientSecret { get; init; } = string.Empty;
    public string GrantType { get; init; } = string.Empty;
    public string Code { get; init; } = string.Empty;
    public string RedirectUri { get; init; } = string.Empty;
}