namespace GameNews.OAuth.Infrastructure.Api.Entities;

public record GuildMemberEntity(
    UserEntity User,
    string Nick,
    string? Avatar,
    List<string> Roles,
    DateTime JoinedAt,
    bool Deaf,
    bool Mute
);