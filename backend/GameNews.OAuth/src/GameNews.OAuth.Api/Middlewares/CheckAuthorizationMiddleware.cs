using System.Net;

namespace GameNews.OAuth.Api.Middlewares;

public class CheckAuthorizationMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.Request.Headers.Authorization.Count > 0
            && context.Request.Headers.Authorization[0]!.Split().Length > 1)
        {
            await next.Invoke(context);
        }
        else
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await context.Response.WriteAsJsonAsync(new
            {
                Error = "invalid access token exception"
            });
        }
    }
}