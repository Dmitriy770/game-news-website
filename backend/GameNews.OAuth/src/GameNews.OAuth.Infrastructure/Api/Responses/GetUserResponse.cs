using GameNews.OAuth.Domain.Entities;

namespace GameNews.OAuth.Infrastructure.Api.Responses;

public record GetUserResponse(
    string[] Scopes,
    string Expires,
    DiscordUserModel User
);