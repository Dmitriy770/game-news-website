using GameNews.OAuth.Domain.Interfaces;
using GameNews.OAuth.Domain.Models;
using GameNews.OAuth.Domain.Services.Interfaces;

namespace GameNews.OAuth.Domain.Services;

public class AuthService : IAuthService
{
    private readonly IDiscordApi _discordApi;

    public AuthService(IDiscordApi discordApi)
    {
        _discordApi = discordApi;
    }

    public async Task<AccessTokenModel> GetAccessToken(string code, CancellationToken cancellationToken)
        => await _discordApi.GetAccessToken(code, cancellationToken);

    public async Task<AccessTokenModel> RefreshAccessToken(string refreshToken, CancellationToken cancellationToken)
        => await _discordApi.RefreshAccessToken(refreshToken, cancellationToken);

    public async Task RevokeAccessToken(string accessToken, CancellationToken cancellationToken)
        => await _discordApi.RevokeAccessToken(accessToken, cancellationToken);

    public async Task<UserModel> GetUser(string accessToken, CancellationToken cancellationToken)
    {
        var user = await _discordApi.GetUser(accessToken, cancellationToken);
        var guilds = await _discordApi.GetUserGuilds(accessToken, cancellationToken);
        var guildMember = await _discordApi.GetGuildMember(accessToken, cancellationToken);

        var avatarUrl = new Uri($"https://cdn.discordapp.com/avatars/{user.Id}/{user.Avatar}");
        
        Console.WriteLine(user);
        Console.WriteLine(string.Join(" ", guilds));
        Console.WriteLine(guildMember);

        return new UserModel(
            user.Id,
            user.Username,
            user.GlobalName,
            avatarUrl
        );
    }
}