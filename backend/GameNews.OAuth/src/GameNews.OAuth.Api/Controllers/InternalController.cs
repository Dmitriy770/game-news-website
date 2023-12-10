using GameNews.OAuth.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
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
    public async Task<IActionResult> ValidateToken(
        [FromHeader] string? authorization,
        CancellationToken cancellationToken)
    {
        if (authorization is null)
        {
            Response.Headers["UserId"] = "";
            Response.Headers["UserRole"] = "User";
            Response.Headers["Username"] = "";
            return new OkResult();
        }
        
        var accessToken = authorization.Replace("Bearer ", "");
        var user = await _authService.GetUser(accessToken, cancellationToken);

        Response.Headers["Username"] = user.Name;
        Response.Headers["UserId"] = user.Id;
        Response.Headers["UserRole"] = user.Role;
        
        return new OkResult();
    }
}