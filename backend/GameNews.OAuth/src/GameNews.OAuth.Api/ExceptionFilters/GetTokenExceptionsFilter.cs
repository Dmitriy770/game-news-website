using GameNews.OAuth.Infrastructure.Api.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GameNews.OAuth.Api.ExceptionFilters;

public class GetTokenExceptionsFilter : Attribute, IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        switch (context.Exception)
        {
            case InvalidCodeException:
                var response = new
                {
                    Title = "Code is invalid",
                    Status = 400,
                };
                context.Result = new JsonResult(response)
                {
                    StatusCode = response.Status
                };
                context.ExceptionHandled = true;
                break;
            case GetTokenException:
                response = new
                {
                    Title = "Get token internal server error",
                    Status = 500,
                };
                context.Result = new JsonResult(response)
                {
                    StatusCode = response.Status
                };
                context.ExceptionHandled = true;
                break;
        }
    }
}