using GameNews.OAuth.Domain.Interfaces;
using GameNews.OAuth.Domain.Services.Interfaces;

namespace GameNews.OAuth.Domain.Services;

public class OAuthService : IOAuthService
{
    private IOAuthApi _OAuthApi;

    public OAuthService(IOAuthApi oAuthApi)
    {
        _OAuthApi = oAuthApi;
    }

    public async Task<string> LogIn(string code, CancellationToken token)
    {
        await _OAuthApi.GetOAuthInfo(code, token);

        return "";
    }
}