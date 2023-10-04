namespace GameNews.OAuth.Domain.Models;

public record AccessTokenModel(
    string AccessToken,
    int ExpiresIn,
    string RefreshToken
);