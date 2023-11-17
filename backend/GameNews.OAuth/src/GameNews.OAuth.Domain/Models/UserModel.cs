namespace GameNews.OAuth.Domain.Models;

public record UserModel(
    string Id,
    string Name,
    Uri AvatarUrl,
    string Role
);