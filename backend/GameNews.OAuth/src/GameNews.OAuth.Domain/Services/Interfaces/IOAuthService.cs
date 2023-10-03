using GameNews.OAuth.Domain.Models;

namespace GameNews.OAuth.Domain.Services.Interfaces;

public interface IOAuthService
{
    public Task<OAuthModel> LogIn(string code, CancellationToken token);
}