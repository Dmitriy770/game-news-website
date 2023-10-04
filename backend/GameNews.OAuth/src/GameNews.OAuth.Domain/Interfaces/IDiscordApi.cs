using GameNews.OAuth.Domain.Models;

namespace GameNews.OAuth.Domain.Interfaces;

public interface IDiscordApi
{
    public Task<AccessTokenModel> GetAccessToken(string code, CancellationToken cancellationToken);

    public Task<UserModel> GetUser(string accessToken, CancellationToken cancellationToken);
}