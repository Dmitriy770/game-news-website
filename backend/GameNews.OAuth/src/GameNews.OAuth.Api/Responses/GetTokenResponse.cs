namespace GameNews.OAuth.Api.Responses;

public record GetTokenResponse(
    string AccessToken,
    int ExpiresIn,
    string RefreshToken
);