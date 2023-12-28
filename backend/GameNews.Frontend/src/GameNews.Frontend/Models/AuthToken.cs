namespace GameNews.Frontend.Models;

public record AuthToken(
    string AccessToken,
    int ExpiresIn,
    string RefreshToken
);