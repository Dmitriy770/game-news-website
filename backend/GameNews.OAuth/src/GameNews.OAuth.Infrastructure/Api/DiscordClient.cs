using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using GameNews.OAuth.Domain.Entities;
using GameNews.OAuth.Domain.Interfaces;
using GameNews.OAuth.Domain.Models;
using GameNews.OAuth.Infrastructure.Api.Exceptions;
using GameNews.OAuth.Infrastructure.Api.Responses;
using GameNews.OAuth.Infrastructure.Options;
using GameNews.OAuth.Infrastructure.Utils;
using Microsoft.Extensions.Options;

namespace GameNews.OAuth.Infrastructure.Api;

public class DiscordClient : IDiscordClient
{
    private readonly HttpClient _httpClient;
    private const string RedirectUri = "http://localhost:8080/login";
    private readonly DiscordApiOptions _discordApiOptions;

    private readonly JsonSerializerOptions _serializerOptions =
        new() { PropertyNamingPolicy = new SnakeCaseNamingPolicy() };

    public DiscordClient(IHttpClientFactory httpClientFactory, IOptions<DiscordApiOptions> discordApiOptions)
    {
        _discordApiOptions = discordApiOptions.Value;
        _httpClient = httpClientFactory.CreateClient("discordApi");
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
            RequestUri = new Uri("oauth2/token", UriKind.RelativeOrAbsolute),
            Method = HttpMethod.Post,
            Content = new FormUrlEncodedContent(content)
        };
        
        var response = await _httpClient.SendAsync(request, cancellationToken);
        if (ExceptionResponseHandler(response) is {} exception)
        {
            throw exception;
        }

        return (await response.Content.ReadFromJsonAsync<AccessTokenModel>(_serializerOptions, cancellationToken))!;
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
            RequestUri = new Uri("oauth2/token/revoke", UriKind.RelativeOrAbsolute),
            Method = HttpMethod.Post,
            Content = new FormUrlEncodedContent(requestBody)
        };

        await _httpClient.SendAsync(request, cancellationToken);
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
            RequestUri = new Uri("oauth2/token", UriKind.RelativeOrAbsolute),
            Method = HttpMethod.Post,
            Content = new FormUrlEncodedContent(content)
        };
        var response = await _httpClient.SendAsync(request, cancellationToken);
        if (ExceptionResponseHandler(response) is {} exception)
        {
            throw exception;
        }

        return (await response.Content.ReadFromJsonAsync<AccessTokenModel>(_serializerOptions, cancellationToken))!;
    }

    public async Task<DiscordUserModel> GetUser(string accessToken, CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage
        {
            RequestUri = new Uri("oauth2/@me", UriKind.RelativeOrAbsolute),
            Method = HttpMethod.Get,
            Headers =
            {
                Authorization = new AuthenticationHeaderValue("Bearer", accessToken)
            }
        };
        var response = await _httpClient.SendAsync(request, cancellationToken);
        if (ExceptionResponseHandler(response) is {} exception)
        {
            throw exception;
        }

        return (await response.Content.ReadFromJsonAsync<GetUserResponse>(_serializerOptions, cancellationToken))!.User;
    }

    public async Task<IEnumerable<DiscordGuildModel>> GetUserGuilds(string accessToken, CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage
        {
            RequestUri = new Uri("users/@me/guilds", UriKind.RelativeOrAbsolute),
            Method = HttpMethod.Get,
            Headers =
            {
                Authorization = new AuthenticationHeaderValue("Bearer", accessToken)
            }
        };
        var response = await _httpClient.SendAsync(request, cancellationToken);
        if (ExceptionResponseHandler(response) is {} exception)
        {
            throw exception;
        }

        return (await response.Content.ReadFromJsonAsync<List<DiscordGuildModel>>(_serializerOptions, cancellationToken))!;
    }

    public async Task<DiscordGuildMemberModel?> GetGuildMember(string accessToken, CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage
        {
            RequestUri = new Uri("users/@me/guilds/734819128916967494/member", UriKind.RelativeOrAbsolute),
            Method = HttpMethod.Get,
            Headers =
            {
                Authorization = new AuthenticationHeaderValue("Bearer", accessToken)
            }
        };
        var response = await _httpClient.SendAsync(request, cancellationToken);
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }
        if (ExceptionResponseHandler(response) is {} exception)
        {
            throw exception;
        }

        return (await response.Content.ReadFromJsonAsync<DiscordGuildMemberModel>(_serializerOptions, cancellationToken))!;
    }

    private Exception? ExceptionResponseHandler(HttpResponseMessage response)
    {
        if (response.StatusCode == HttpStatusCode.OK)
        {
            return null;
        }

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return new NotFoundException();
        }

        return new DiscordApiException();
    }
}