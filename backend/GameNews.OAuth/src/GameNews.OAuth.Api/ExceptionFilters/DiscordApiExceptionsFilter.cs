using System.Net;
using GameNews.OAuth.Infrastructure.Api.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GameNews.OAuth.Api.ExceptionFilters;

public class DiscordApiExceptionsFilter : Attribute, IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        switch (context.Exception)
        {
            case InvalidCodeException:
                var result = new JsonResult(new
                {
                    Error = "invalid code exception"
                });
                result.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Result = result;
                context.ExceptionHandled = true;
                break;
            case InvalidTokenException:
                result = new JsonResult(new
                {
                    Error = "invalid access token exception"
                });
                result.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Result = result;
                context.ExceptionHandled = true;
                break;
            case DiscordApiException:
                result = new JsonResult(new
                {
                    Error = "discord api error"
                });
                result.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Result = result;
                context.ExceptionHandled = true;
                break;
        }
    }
}