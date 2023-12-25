using FluentResults;
using GameNews.Articles.Api.Exceptions;
using GameNews.Articles.Domain.Errors;
using Microsoft.AspNetCore.Mvc;

namespace GameNews.Articles.Api.Extensions;

public static class ListErrorsToActionResult
{
    public static ActionResult ToActionResult(this List<IError> errors)
    {
        if (errors.Find(e => e is AccessDeniedError) is not null)
        {
            return new ForbidResult();
        }

        if (errors.Find(e => e is ArticleNotFoundError or TagNotFoundError) is not null)
        {
            return new NotFoundResult();
        }

        if (errors.Find(e => e is ValidateError or DeleteTagError) is not null)
        {
            return new BadRequestResult();
        }

        throw new UnhandledErrorException();
    }
}