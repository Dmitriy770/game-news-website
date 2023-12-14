using FluentResults;
using GameNews.Articles.Domain.Errors;

namespace GameNews.Articles.Domain.ValueTypes;

public record RoleType
{
    public string Value { get; }

    private RoleType(string value)
    {
        Value = value;
    }

    public static Result<RoleType> Create(string role)
    {
        role = role.Trim();

        if (role is not ("User" or "Author" or "Administrator"))
        {
            return Result.Fail(new ValidateError("Invalid role"));
        }

        return new RoleType(role);
    }
}