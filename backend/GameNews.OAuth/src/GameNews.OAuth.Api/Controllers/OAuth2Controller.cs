﻿using GameNews.OAuth.Api.ExceptionFilters;
using GameNews.OAuth.Api.Responses.V1;
using GameNews.OAuth.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GameNews.OAuth.Api.Controllers;

[ApiController]
[Route("api/v1/oauth2")]
public class OAuth2Controller : ControllerBase
{
    private const string UrlOauth2Redirect =
        "https://discord.com/api/oauth2/authorize?client_id=742333635130163270&redirect_uri=http%3A%2F%2Flocalhost%3A8080%2Fapi%2Fv1%2Foauth2%2Fauthorize&response_type=code&scope=identify%20guilds";

    private readonly IOAuth2Service _oAuth2Service;

    public OAuth2Controller(IOAuth2Service oAuth2Service)
    {
        _oAuth2Service = oAuth2Service;
    }

    [HttpGet("authorize")]
    public IActionResult Login([FromQuery] string? code, [FromQuery] string? state)
    {
        if (code is not null && state is not null)
        {
            return Redirect($"{state}?code={code}");
        }

        return Redirect($"{UrlOauth2Redirect}&state={HttpContext.Request.Headers.Referer}");
    }

    [HttpGet("token")]
    [DiscordApiExceptionsFilter]
    public async Task<GetTokenResponse> GetToken([FromQuery] string code, CancellationToken cancellationToken)
    {
        var token = await _oAuth2Service.GetAccessToken(code, cancellationToken);

        return new GetTokenResponse
        {
            AccessToken = token.AccessToken,
            ExpiresIn = token.ExpiresIn,
            RefreshToken = token.RefreshToken
        };
    }

    [HttpGet("token/refresh")]
    [DiscordApiExceptionsFilter]
    public async Task<RefreshTokenResponse> RefreshToken([FromQuery(Name = "token")] string refreshToken,
        CancellationToken cancellationToken)
    {
        var accessToken = await _oAuth2Service.RefreshAccessToken(refreshToken, cancellationToken);

        return new RefreshTokenResponse
        {
            AccessToken = accessToken.AccessToken,
            ExpiresIn = accessToken.ExpiresIn,
            RefreshToken = accessToken.RefreshToken
        };
    }

    [HttpGet("token/revoke")]
    [DiscordApiExceptionsFilter]
    public async Task<RevokeTokenResponse> RevokeToken([FromQuery(Name = "token")] string accessToken,
        CancellationToken cancellationToken)
    {
        await _oAuth2Service.RevokeAccessToken(accessToken, cancellationToken);

        return new RevokeTokenResponse();
    }

    [HttpGet("me")]
    [DiscordApiExceptionsFilter]
    public async Task<GetUserResponse> GetMe(CancellationToken cancellationToken)
    {
        var accessToken = Request.Headers.Authorization[0]!.Split()[1];

        var user = await _oAuth2Service.GetUser(accessToken, cancellationToken);

        return new GetUserResponse
        (
            user.GlobalName,
            user.AvatarUrl.ToString()
        );
    }
}