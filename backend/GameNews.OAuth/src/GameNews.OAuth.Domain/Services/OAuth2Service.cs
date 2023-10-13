using GameNews.OAuth.Domain.Interfaces;
using GameNews.OAuth.Domain.Models;
using GameNews.OAuth.Domain.Services.Interfaces;

namespace GameNews.OAuth.Domain.Services;

public class OAuth2Service : IOAuth2Service
{
    private readonly IDiscordApi _discordApi;

    public OAuth2Service(IDiscordApi discordApi)
    {
        _discordApi = discordApi;
    }

    public async Task<AccessTokenModel> GetAccessToken(string code, CancellationToken cancellationToken)
    {
        var accessToken = await _discordApi.GetAccessToken(code, cancellationToken);

        return accessToken;
    }

    public async Task<UserModel> GetUser(string accessToken, CancellationToken cancellationToken)
    {
        var user = await _discordApi.GetUser(accessToken, cancellationToken);

        return user;
    }

    public async Task<AccessTokenModel> RefreshAccessToken(string refreshToken, CancellationToken cancellationToken)
    {
        var accessToken = await _discordApi.RefreshAccessToken(refreshToken, cancellationToken);

        return accessToken;
    }

    public async Task RevokeAccessToken(string accessToken, CancellationToken cancellationToken)
    {
        await _discordApi.RevokeAccessToken(accessToken, cancellationToken);
    }
}