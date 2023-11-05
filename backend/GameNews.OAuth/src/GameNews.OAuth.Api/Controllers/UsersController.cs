using GameNews.OAuth.Api.Responses;
using GameNews.OAuth.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GameNews.OAuth.Api.Controllers;

[ApiController]
[Route("users")]
public class UsersController : ControllerBase
{
    private readonly IAuthService _authService;

    public UsersController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpGet("me")]
    public async Task<GetUserResponse> GetMe([FromHeader] string authorization, CancellationToken cancellationToken)
    {
        var accessToken = authorization.Replace("Bearer ", "");

        var user = await _authService.GetUser(accessToken, cancellationToken);

        return new GetUserResponse
        (
            user.GlobalName,
            user.AvatarUrl.ToString()
        );
    }
}