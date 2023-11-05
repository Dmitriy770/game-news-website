namespace GameNews.OAuth.Infrastructure.Api.Entities;

public record UserEntity(
    string Id,
    string Username,
    string Avatar,
    string Discriminator,
    string GlobalName,
    int PublicFlags 
);