using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using GameNews.Frontend.Models;
using GameNews.Frontend.Services.Interfaces;

namespace GameNews.Frontend.Services;

public class UserService(
    HttpClient httpClient
) : IUserService
{
    public async Task<User?> GetMe(string? accessToken, CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage
        {
            RequestUri = new Uri("api/auth/users/me", UriKind.RelativeOrAbsolute),
            Method = HttpMethod.Get,
            Headers =
            {
                Authorization = new AuthenticationHeaderValue("Bearer", accessToken)
            }
        };

        var response = await httpClient.SendAsync(request, cancellationToken);
        if (response.StatusCode != HttpStatusCode.OK)
        {
            return null;
        }

        return await response.Content.ReadFromJsonAsync<User>(cancellationToken);
    }
}