using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using GameNews.OAuth.Domain.Interfaces;
using GameNews.OAuth.Domain.Models;
using GameNews.OAuth.Infrastructure.Api.Exceptions;
using GameNews.OAuth.Infrastructure.Api.Responses;
using GameNews.OAuth.Infrastructure.Api.Utils;
using GameNews.OAuth.Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace GameNews.OAuth.Infrastructure.Api;

public class DiscordApi : IDiscordApi
{
    private static readonly HttpClient HttpClient = new();
    private const string ApiUrl = "https://discord.com/api/v10";
    private const string RedirectUri = "http://localhost:8080/api/v1/oauth2/authorize";
    private readonly DiscordApiOptions _discordApiOptions;

    public DiscordApi(IOptions<DiscordApiOptions> discordApiOptions)
    {
        _discordApiOptions = discordApiOptions.Value;
    }

    public async Task<AccessTokenModel> GetAccessToken(string code, CancellationToken cancellationToken)
    {
        var oauthInfo = new KeyValuePair<string, string>[]
        {
            new("client_id", _discordApiOptions.ClientId),
            new("client_secret", _discordApiOptions.ClientSecret),
            new("grant_type", "authorization_code"),
            new("redirect_uri", RedirectUri),
            new("code", code)
        };

        var request = new HttpRequestMessage
        {
            RequestUri = new Uri(ApiUrl + "/v10/oauth2/token"),
            Method = HttpMethod.Post,
            Content = new FormUrlEncodedContent(oauthInfo)
        };

        var response = await HttpClient.SendAsync(request, cancellationToken);

        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
                var serializerOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = new SnakeCaseNamingPolicy()
                };
                var body = await response.Content.ReadFromJsonAsync<GetTokenResponse>(serializerOptions,
                    cancellationToken);

                return new AccessTokenModel
                (
                    body.AccessToken,
                    body.ExpiresIn,
                    body.RefreshToken
                );
            case HttpStatusCode.BadRequest:
                throw new InvalidCodeException();
            default:
                throw new DiscordApiException();
        }
    }

    public async Task<UserModel> GetUser(string accessToken, CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage
        {
            RequestUri = new Uri(ApiUrl + "/oauth2/@me"),
            Method = HttpMethod.Get,
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await HttpClient.SendAsync(request, cancellationToken);

        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
                var serializerOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = new SnakeCaseNamingPolicy()
                };
                var body = await response.Content.ReadFromJsonAsync<GetUserResponse>(serializerOptions,
                    cancellationToken);

                var avatarUrl = new Uri($"https://cdn.discordapp.com/avatars/{body.User.Id}/{body.User.Avatar}");

                return new UserModel(
                    body.User.Id,
                    body.User.Username,
                    avatarUrl,
                    body.User.GlobalName
                );
            case HttpStatusCode.Unauthorized:
                throw new InvalidTokenException();
            default:
                throw new DiscordApiException();
        }
    }
}