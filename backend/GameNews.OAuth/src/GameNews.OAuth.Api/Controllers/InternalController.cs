using GameNews.OAuth.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GameNews.OAuth.Api.Controllers;

[ApiController]
[Route("internal")]
public class InternalController : ControllerBase
{
    private readonly IAuthService _authService;

    public InternalController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpGet("token/validate")]
    public async Task<IActionResult> ValidateToken([FromHeader]string authorization, CancellationToken cancellationToken)
    {
        var accessToken = authorization.Replace("Bearer ", "");
        await _authService.GetUser(accessToken, cancellationToken);

        Response.Headers["Username"] = "TestUsername";
        Response.Headers["UserId"] = "TestUserId";
        Response.Headers["UserPermission"] = "3";
        return new UnauthorizedResult();
    }
}