using GameNews.Frontend.Models;

namespace GameNews.Frontend.Services.Interfaces;

public interface IUserService
{
    public Task<User?> GetMe(string? accessToken, CancellationToken cancellationToken = default);

}