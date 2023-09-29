using GameNews.OAuth.Domain.Interfaces;
using GameNews.OAuth.Domain.Models;

namespace GameNews.OAuth.Infrastructure.Api.Discord;

public class OAuthApi : IOAuthApi
{
    private static readonly HttpClient _httpClient = new();

    public async Task<OAuthModel> GetOAuthInfo(string code, CancellationToken token)
    {
        var oauthInfo = new KeyValuePair<string, string>[]
        {
            new("client_id", "742333635130163270"),
            new("client_secret", ""),
            new("grant_type", "authorization_code"),
            new("redirect_uri", "http://localhost:8080/api/v1/oauth/login"),
            new("code", code)
        };

        var request = new HttpRequestMessage
        {
            RequestUri = new Uri("https://discord.com/api/v10/oauth2/token"),
            Method = HttpMethod.Post,
            Content = new FormUrlEncodedContent(oauthInfo)
        };
        // request.Headers.Add("Content-Type","application/x-www-form-urlencoded");

        var response = await _httpClient.SendAsync(request, token);

        Console.WriteLine(await response.Content.ReadAsStringAsync(token));

        return new OAuthModel();
    }
}