using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using GameNews.OAuth.Domain.Interfaces;
using GameNews.OAuth.Domain.Models;
using GameNews.OAuth.Infrastructure.Api.Entities;
using GameNews.OAuth.Infrastructure.Api.Responses;
using GameNews.OAuth.Infrastructure.Options;
using GameNews.OAuth.Infrastructure.Utils;
using Microsoft.Extensions.Options;

namespace GameNews.OAuth.Infrastructure.Api;

public class DiscordApi : IDiscordApi
{
    private static readonly HttpClient HttpClient = new();
    private const string ApiUrl = "https://discord.com/api/v10";
    private const string RedirectUri = "http://localhost:8080/api/v1/oauth2/authorize";
    private readonly DiscordApiOptions _discordApiOptions;

    private readonly JsonSerializerOptions _serializerOptions =
        new() { PropertyNamingPolicy = new SnakeCaseNamingPolicy() };

    public DiscordApi(IOptions<DiscordApiOptions> discordApiOptions)
    {
        _discordApiOptions = discordApiOptions.Value;
    }

    public async Task<AccessTokenModel> GetAccessToken(string code, CancellationToken cancellationToken)
    {
        var content = new KeyValuePair<string, string>[]
        {
            new("client_id", _discordApiOptions.ClientId),
            new("client_secret", _discordApiOptions.ClientSecret),
            new("grant_type", "authorization_code"),
            new("redirect_uri", RedirectUri),
            new("code", code)
        };
        var request = new HttpRequestMessage
        {
            RequestUri = new Uri(ApiUrl + "/oauth2/token"),
            Method = HttpMethod.Post,
            Content = new FormUrlEncodedContent(content)
        };
        var response = await HttpClient.SendAsync(request, cancellationToken);
        if (response.StatusCode != HttpStatusCode.OK)
        {
            throw ErrorResponseHandler(response);
        }

        var body = await response.Content.ReadFromJsonAsync<GetTokenResponse>(_serializerOptions, cancellationToken);

        return new AccessTokenModel
        (
            body.AccessToken,
            body.ExpiresIn,
            body.RefreshToken
        );
    }

    public async Task RevokeAccessToken(string accessToken, CancellationToken cancellationToken)
    {
        var requestBody = new KeyValuePair<string, string>[]
        {
            new("client_id", _discordApiOptions.ClientId),
            new("client_secret", _discordApiOptions.ClientSecret),
            new("token", accessToken),
            new("token_type_hint", "access_token")
        };

        var request = new HttpRequestMessage
        {
            RequestUri = new Uri(ApiUrl + "/oauth2/token/revoke"),
            Method = HttpMethod.Post,
            Content = new FormUrlEncodedContent(requestBody)
        };

        await HttpClient.SendAsync(request, cancellationToken);
    }

    public async Task<AccessTokenModel> RefreshAccessToken(string accessToken, CancellationToken cancellationToken)
    {
        var content = new KeyValuePair<string, string>[]
        {
            new("client_id", _discordApiOptions.ClientId),
            new("client_secret", _discordApiOptions.ClientSecret),
            new("token", accessToken),
            new("token_type_hint", "access_token")
        };
        var request = new HttpRequestMessage
        {
            RequestUri = new Uri(ApiUrl + "/oauth/token"),
            Method = HttpMethod.Post,
            Content = new FormUrlEncodedContent(content)
        };
        var response = await HttpClient.SendAsync(request, cancellationToken);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            throw ErrorResponseHandler(response);
        }

        var body = await response.Content.ReadFromJsonAsync<GetTokenResponse>(_serializerOptions, cancellationToken);

        return new AccessTokenModel
        (
            body.AccessToken,
            body.ExpiresIn,
            body.RefreshToken
        );
    }

    public async Task<UserEntity> GetUser(string accessToken, CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage
        {
            RequestUri = new Uri(ApiUrl + "/oauth2/@me"),
            Method = HttpMethod.Get,
            Headers =
            {
                Authorization = new AuthenticationHeaderValue("Bearer", accessToken)
            }
        };
        var response = await HttpClient.SendAsync(request, cancellationToken);
        if (response.StatusCode != HttpStatusCode.OK)
        {
            throw ErrorResponseHandler(response);
        }

        var content  = (await response.Content.ReadFromJsonAsync<GetUserResponse>(_serializerOptions, cancellationToken))!;
        
        return content.User;
    }

    public async Task<List<GuildEntity>> GetUserGuilds(string accessToken, CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage
        {
            RequestUri = new Uri(ApiUrl + "/users/@me/guilds"),
            Method = HttpMethod.Get,
            Headers =
            {
                Authorization = new AuthenticationHeaderValue("Bearer", accessToken)
            }
        };
        var response = await HttpClient.SendAsync(request, cancellationToken);
        if (response.StatusCode != HttpStatusCode.OK)
        {
            throw ErrorResponseHandler(response);
        }

        return (await response.Content.ReadFromJsonAsync<List<GuildEntity>>(_serializerOptions, cancellationToken))!;
    }

    public async Task<GuildMemberEntity> GetGuildMember(string accessToken, CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage
        {
            RequestUri = new Uri(ApiUrl + "/users/@me/guilds/734819128916967494/member"),
            Method = HttpMethod.Get,
            Headers =
            {
                Authorization = new AuthenticationHeaderValue("Bearer", accessToken)
            }
        };
        var response = await HttpClient.SendAsync(request, cancellationToken);
        if (response.StatusCode != HttpStatusCode.OK)
        {
            throw ErrorResponseHandler(response);
        }

        return (await response.Content.ReadFromJsonAsync<GuildMemberEntity>(_serializerOptions, cancellationToken))!;
    }

    private Exception ErrorResponseHandler(HttpResponseMessage response)
    {
        Console.WriteLine(response.StatusCode);
        return new NotImplementedException();
    }
}