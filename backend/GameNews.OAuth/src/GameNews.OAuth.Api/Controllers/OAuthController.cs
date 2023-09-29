using GameNews.OAuth.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace GameNews.OAuth.Api.Controllers;

[ApiController]
[Route("api/v1/oauth")]
public class OAuthController : ControllerBase
{
    private const string URL_OAUTH_REDIRECT = "https://discord.com/api/oauth2/authorize?client_id=742333635130163270&redirect_uri=http%3A%2F%2Flocalhost%3A8080%2Fapi%2Fv1%2Foauth%2Flogin&response_type=code&scope=identify";
    private IOAuthService _OAuthService;

    public OAuthController(IOAuthService oAuthService)
    {
        _OAuthService = oAuthService;
    }
    
    [HttpGet("login")]
    public async Task<IActionResult> Login([FromQuery]string? code, [FromQuery]string? state, CancellationToken token)
    {
        if (code is not null && state is not null)
        {
            await _OAuthService.LogIn(code, token);
            
            return Redirect(state);
        }

        return Redirect($"{URL_OAUTH_REDIRECT}&state={HttpContext.Request.Headers.Referer}");
    }
}