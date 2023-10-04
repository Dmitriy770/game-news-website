using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using GameNews.OAuth.Domain.Interfaces;
using GameNews.OAuth.Domain.Models;
using GameNews.OAuth.Infrastructure.Api.Exceptions;
using GameNews.OAuth.Infrastructure.Api.Responses;
using GameNews.OAuth.Infrastructure.Api.Utils;

namespace GameNews.OAuth.Infrastructure.Api;

public class DiscordApi : IDiscordApi
{
    private static readonly HttpClient HttpClient = new();

    public async Task<AccessTokenModel> GetAccessToken(string code, CancellationToken cancellationToken)
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

        var response = await HttpClient.SendAsync(request, cancellationToken);

        var serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = new SnakeCaseNamingPolicy()
        };

        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            throw new InvalidCodeException();
        }

        if (response.StatusCode != HttpStatusCode.OK)
        {
            throw new GetTokenException();
        }
        
        var body = await response.Content.ReadFromJsonAsync<GetTokenResponse>(serializerOptions, cancellationToken);
        
        return new AccessTokenModel
        (
            body.AccessToken,
            body.ExpiresIn,
            body.RefreshToken
        );
    }

    public async Task<UserModel> GetUser(string accessToken, CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage
        {
            RequestUri = new Uri("https://discord.com/api/v10/oauth2/@me"),
            Method = HttpMethod.Get,
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await HttpClient.SendAsync(request, cancellationToken);

        var serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = new SnakeCaseNamingPolicy()
        };
        var body = await response.Content.ReadFromJsonAsync<GetUserResponse>(serializerOptions, cancellationToken);

        var avatarUrl = new Uri($"https://cdn.discordapp.com/avatars/{body.User.Id}/{body.User.Avatar}");

        return new UserModel(
            body.User.Id,
            body.User.Username,
            avatarUrl,
            body.User.GlobalName
        );
    }
}