namespace GameNews.OAuth.Infrastructure.Api.Entities;

public record GuildEntity(
    string Id,
    string Name,
    string Icon,
    bool Owner,
    string Permissions,
    List<string> Features,
    int ApproximateMemberCount,
    int ApproximatePresenceCount
);