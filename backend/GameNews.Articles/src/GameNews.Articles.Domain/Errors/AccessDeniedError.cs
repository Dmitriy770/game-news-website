using FluentResults;

namespace GameNews.Articles.Domain.Errors;

public sealed class AccessDeniedError : IError
{
    public string Message { get; } = "Access denied error";
    public Dictionary<string, object> Metadata { get; } = new();
    public List<IError> Reasons { get; } = [];
}