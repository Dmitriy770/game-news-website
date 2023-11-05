namespace GameNews.OAuth.Infrastructure.Api.Entities;

public record RoleEntity(
    string Id,
    string Name,
    string Permissions,
    string Position,
    int Color,
    bool Hoist,
    bool Managed,
    bool Mentionable
    );