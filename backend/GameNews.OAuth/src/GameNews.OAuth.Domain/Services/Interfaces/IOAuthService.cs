namespace GameNews.OAuth.Domain.Services.Interfaces;

public interface IOAuthService
{
    public Task<string> LogIn(string code, CancellationToken token);
}