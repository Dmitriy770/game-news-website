using GameNews.OAuth.Domain.Interfaces;
using GameNews.OAuth.Domain.Models;
using GameNews.OAuth.Domain.Services.Interfaces;

namespace GameNews.OAuth.Domain.Services;

public class AuthService : IAuthService
{
    private readonly IDiscordClient _discordClient;

    public AuthService(IDiscordClient discordClient)
    {
        _discordClient = discordClient;
    }

    public async Task<AccessTokenModel> GetAccessToken(string code, CancellationToken cancellationToken)
        => await _discordClient.GetAccessToken(code, cancellationToken);

    public async Task<AccessTokenModel> RefreshAccessToken(string refreshToken, CancellationToken cancellationToken)
        => await _discordClient.RefreshAccessToken(refreshToken, cancellationToken);

    public async Task RevokeAccessToken(string accessToken, CancellationToken cancellationToken)
        => await _discordClient.RevokeAccessToken(accessToken, cancellationToken);

    public async Task<UserModel> GetUser(string accessToken, CancellationToken cancellationToken)
    {
        var user = await _discordClient.GetUser(accessToken, cancellationToken);
        var guilds = await _discordClient.GetUserGuilds(accessToken, cancellationToken);
        var guildMember = await _discordClient.GetGuildMember(accessToken, cancellationToken);

        var guild = guilds.FirstOrDefault(g => g.Id.CompareTo("734819128916967494") == 0);
        var role = "User";
        if (guildMember is not null && guildMember.Roles.Any(r => r.CompareTo("789511663459369001") == 0))
        {
            role = "Author";
        }
        else if (guild is not null && guild.Owner)
        {
            role = "Administrator";
        }

        return new UserModel(
            user.Id,
            user.GlobalName,
            new Uri($"https://cdn.discordapp.com/avatars/{user.Id}/{user.Avatar}"),
            role
        );
    }
}