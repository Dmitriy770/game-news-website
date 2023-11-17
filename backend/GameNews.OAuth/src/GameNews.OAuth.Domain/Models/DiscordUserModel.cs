namespace GameNews.OAuth.Domain.Entities;

public record DiscordUserModel(
    string Id,
    string Username,
    string Avatar,
    string Discriminator,
    string GlobalName,
    int PublicFlags 
);