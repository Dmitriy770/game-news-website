using GameNews.OAuth.Api.Responses;
using GameNews.OAuth.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GameNews.OAuth.Api.Controllers;

[ApiController]
[Route("token")]
public class TokensController : ControllerBase
{
    private readonly IAuthService _authService;

    public TokensController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpGet]
    public async Task<GetTokenResponse> GetToken([FromQuery] string code, CancellationToken cancellationToken)
    {
        var token = await _authService.GetAccessToken(code, cancellationToken);

        return new GetTokenResponse(
            token.AccessToken,
            token.ExpiresIn,
            token.RefreshToken
        );
    }

    [HttpGet("refresh")]
    public async Task<RefreshTokenResponse> RefreshToken([FromQuery(Name = "token")] string refreshToken,
        CancellationToken cancellationToken)
    {
        var accessToken = await _authService.RefreshAccessToken(refreshToken, cancellationToken);

        return new RefreshTokenResponse(
            accessToken.AccessToken,
            accessToken.ExpiresIn,
            accessToken.RefreshToken
        );
    }

    [HttpGet("revoke")]
    public async Task<RevokeTokenResponse> RevokeToken([FromQuery(Name = "token")] string accessToken,
        CancellationToken cancellationToken)
    {
        await _authService.RevokeAccessToken(accessToken, cancellationToken);

        return new RevokeTokenResponse();
    }
}