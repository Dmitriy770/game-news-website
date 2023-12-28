using System.Net;
using System.Net.Http.Json;
using Blazored.LocalStorage;
using GameNews.Frontend.Models;
using GameNews.Frontend.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace GameNews.Frontend.Services;

public class AuthService(
    ILocalStorageService localStorage,
    HttpClient httpClient,
    NavigationManager navigation
) : IAuthService
{
    public async Task<AuthToken?> GetAuthToken(CancellationToken cancellationToken = default)
    {
        return await localStorage.GetItemAsync<AuthToken?>(nameof(AuthToken), cancellationToken);
    }

    public async ValueTask<bool> LogIn(string code, CancellationToken cancellationToken = default)
    {
        var tokenResult = await httpClient.GetAsync($"api/auth/token?code={code}", cancellationToken);
        if (tokenResult.StatusCode != HttpStatusCode.OK)
        {
            return false;
        }

        var authToken = await tokenResult.Content.ReadFromJsonAsync<AuthToken>(cancellationToken);
        if (authToken is null)
        {
            return false;
        }
        Console.WriteLine(authToken);
        await localStorage.SetItemAsync(nameof(AuthToken), authToken, cancellationToken);
        return true;
    }

    public async Task LogOut(CancellationToken cancellationToken = default)
    {
        await localStorage.RemoveItemAsync(nameof(AuthToken), cancellationToken);
        navigation.NavigateTo("login");
    }

    public async ValueTask<string?> GetActualAccessToken(CancellationToken cancellationToken = default)
    {
        return (await localStorage.GetItemAsync<AuthToken?>(nameof(AuthToken), cancellationToken))?.AccessToken;
    }
}