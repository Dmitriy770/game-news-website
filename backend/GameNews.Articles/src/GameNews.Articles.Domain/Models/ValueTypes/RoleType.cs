using FluentResults;
using GameNews.Articles.Domain.Errors;

namespace GameNews.Articles.Domain.Models.ValueTypes;

public record RoleType
{
    public const string User = "User";
    public const string Author = "Author";
    public const string Administrator = "Administrator";
    public string Value { get; }

    private RoleType(string value)
    {
        Value = value;
    }

    public static Result<RoleType> Create(string role)
    {
        role = role.Trim();

        if (role is not (User or Author or Administrator))
        {
            return Result.Fail(new ValidateError("Invalid role"));
        }

        return new RoleType(role);
    }
}