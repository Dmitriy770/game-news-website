using GameNews.Frontend.Models;

namespace GameNews.Frontend.Services.Interfaces;

public interface IAuthService
{
    public Task<AuthToken?> GetAuthToken(CancellationToken cancellationToken = default);

    public ValueTask<bool> LogIn(string code, CancellationToken cancellationToken = default);

    public ValueTask<string?> GetActualAccessToken(CancellationToken cancellationToken = default);

    public Task LogOut(CancellationToken cancellationToken = default);


}