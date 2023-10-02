namespace GameNews.OAuth.Infrastructure.Api.Discord.Response;

public record TokenResponse(
    string AccessToken,
    string TokenType,
    int ExpiresIn,
    string RefreshToken,
    string Scope
);