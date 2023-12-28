namespace GameNews.Frontend.Models;

public record User(
    string Id,
    string Name,
    Uri AvatarUrl,
    string Role
);