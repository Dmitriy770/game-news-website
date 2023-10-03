using System.Net.Http.Json;
using System.Text.Json;
using GameNews.OAuth.Domain.Interfaces;
using GameNews.OAuth.Domain.Models;
using GameNews.OAuth.Infrastructure.Api.Discord.Response;
using GameNews.OAuth.Infrastructure.Api.Utils;

namespace GameNews.OAuth.Infrastructure.Api.Discord;

public class OAuthApi : IOAuthApi
{
    private static readonly HttpClient _httpClient = new();

    public async Task<OAuthModel> GetOAuthInfo(string code, CancellationToken token)
    {
        var oauthInfo = new KeyValuePair<string, string>[]
        {
            new("client_id", "742333635130163270"),
            new("client_secret", "AOMoJjYXL-bLivTVlp7xtxMIWBrEXvST"),
            new("grant_type", "authorization_code"),
            new("redirect_uri", "http://localhost:8080/api/v1/oauth2/authorize"),
            new("code", code)
        };

        var request = new HttpRequestMessage
        {
            RequestUri = new Uri("https://discord.com/api/v10/oauth2/token"),
            Method = HttpMethod.Post,
            Content = new FormUrlEncodedContent(oauthInfo)
        };

        var response = await _httpClient.SendAsync(request, token);

        var serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = new SnakeCaseNamingPolicy()
        };
        var body = await response.Content.ReadFromJsonAsync<TokenResponse>(serializerOptions, token);

        Console.WriteLine(await response.Content.ReadAsStringAsync());

        return new OAuthModel
        {
            AccessToken = body.AccessToken,
            ExpiresIn = body.ExpiresIn,
            RefreshToken = body.RefreshToken
        };
    }
}