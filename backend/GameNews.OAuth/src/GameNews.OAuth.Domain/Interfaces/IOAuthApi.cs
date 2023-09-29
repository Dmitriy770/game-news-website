using GameNews.OAuth.Domain.Models;

namespace GameNews.OAuth.Domain.Interfaces;

public interface IOAuthApi
{
    public Task<OAuthModel> GetOAuthInfo(string code, CancellationToken token);
}