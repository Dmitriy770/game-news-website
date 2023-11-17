namespace GameNews.OAuth.Domain.Entities;

public record DiscordRoleModel(
    string Id,
    string Name,
    string Permissions,
    string Position,
    int Color,
    bool Hoist,
    bool Managed,
    bool Mentionable
    );