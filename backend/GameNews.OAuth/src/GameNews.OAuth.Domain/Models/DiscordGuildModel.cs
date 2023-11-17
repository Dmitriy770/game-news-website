namespace GameNews.OAuth.Domain.Entities;

public record DiscordGuildModel(
    string Id,
    string Name,
    string Icon,
    bool Owner,
    string Permissions,
    List<string> Features,
    int ApproximateMemberCount,
    int ApproximatePresenceCount
);