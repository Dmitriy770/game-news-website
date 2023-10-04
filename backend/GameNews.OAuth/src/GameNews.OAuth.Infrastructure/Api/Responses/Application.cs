namespace GameNews.OAuth.Infrastructure.Api.Responses;

public record Application(
    string Id,
    string Name,
    string Icon,
    string Description,
    bool Hook,
    bool BotPublic,
    bool BotRequireCodeGrant,
    string VerifyKey
);