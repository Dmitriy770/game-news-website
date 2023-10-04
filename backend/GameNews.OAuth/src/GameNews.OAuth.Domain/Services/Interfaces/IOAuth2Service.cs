using GameNews.OAuth.Domain.Models;

namespace GameNews.OAuth.Domain.Services.Interfaces;

public interface IOAuth2Service
{
    public Task<AccessTokenModel> GetAccessToken(string code, CancellationToken cancellationToken);

    public Task<UserModel> GetUser(string accessToken, CancellationToken cancellationToken);
}