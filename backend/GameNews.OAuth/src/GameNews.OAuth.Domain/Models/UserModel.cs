namespace GameNews.OAuth.Domain.Models;

public record UserModel(
    string Id,
    string Username,
    Uri AvatarUrl,
    string GlobalName
);