namespace GameNews.OAuth.Api.Responses;

public record RefreshTokenResponse(
    string AccessToken,
    int ExpiresIn,
    string RefreshToken
);