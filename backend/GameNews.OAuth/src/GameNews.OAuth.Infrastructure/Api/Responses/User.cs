namespace GameNews.OAuth.Infrastructure.Api.Responses;

public record User(
    string Id,
    string Username,
    string Avatar,
    string Discriminator,
    string GlobalName,
    int PublicFlags 
);