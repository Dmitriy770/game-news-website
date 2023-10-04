namespace GameNews.OAuth.Infrastructure.Api.Responses;

public record GetTokenResponse(
    string AccessToken,
    string TokenType,
    int ExpiresIn,
    string RefreshToken,
    string Scope
);