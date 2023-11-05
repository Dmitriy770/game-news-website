using System.Net;
using GameNews.OAuth.Infrastructure.Api.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace GameNews.OAuth.Api.Controllers;

public class ErrorsController : ControllerBase
{
    [Route("/error")]
    public IActionResult Error()
    {
        var exception = HttpContext.Features.Get<Exception>();

        return exception switch
        {
            InvalidCodeException => Problem("Invalid code", statusCode: (int)HttpStatusCode.BadRequest),
            InvalidTokenException => new UnauthorizedResult(),
            DiscordApiException => Problem("Discord api exception", statusCode: (int)HttpStatusCode.InternalServerError),
            _ => Problem()
        };
    }
}