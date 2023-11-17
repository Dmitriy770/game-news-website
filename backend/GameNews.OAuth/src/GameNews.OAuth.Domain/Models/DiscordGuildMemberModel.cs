namespace GameNews.OAuth.Domain.Entities;

public record DiscordGuildMemberModel(
    DiscordUserModel DiscordUser,
    string Nick,
    string? Avatar,
    List<string> Roles,
    DateTime JoinedAt,
    bool Deaf,
    bool Mute
);