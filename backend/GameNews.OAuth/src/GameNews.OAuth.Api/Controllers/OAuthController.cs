using GameNews.OAuth.Api.Responses.V1;
using GameNews.OAuth.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GameNews.OAuth.Api.Controllers;

[ApiController]
[Route("api/v1/oauth2")]
public class OAuthController : ControllerBase
{
    private const string URL_OAUTH_REDIRECT =
        "https://discord.com/api/oauth2/authorize?client_id=742333635130163270&redirect_uri=http%3A%2F%2Flocalhost%3A8080%2Fapi%2Fv1%2Foauth2%2Fauthorize&response_type=code&scope=identify%20guilds";

    private IOAuthService _OAuthService;

    public OAuthController(IOAuthService oAuthService)
    {
        _OAuthService = oAuthService;
    }

    [HttpGet("authorize")]
    public IActionResult Login([FromQuery] string? code, [FromQuery] string? state)
    {
        if (code is not null && state is not null)
        {
            return Redirect($"{state}?code={code}");
        }

        return Redirect($"{URL_OAUTH_REDIRECT}&state={HttpContext.Request.Headers.Referer}");
    }

    [HttpGet("token")]
    public async Task<GetTokenResponse> GetToken([FromQuery] string code, CancellationToken cancellationToken)
    {
        var token = await _OAuthService.LogIn(code, cancellationToken);
        
        Console.WriteLine(token);
        
        return new GetTokenResponse
        {
            AccessToken = token.AccessToken,
            ExpiresIn = token.ExpiresIn,
            RefreshToken = token.RefreshToken
        };
    }
}