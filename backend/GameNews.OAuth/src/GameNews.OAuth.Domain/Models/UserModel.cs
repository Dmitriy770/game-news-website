namespace GameNews.OAuth.Domain.Models;

public record UserModel(
    string Id,
    string Username,
    string GlobalName,
    Uri AvatarUrl
);