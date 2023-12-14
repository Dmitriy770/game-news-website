using FluentResults;

namespace GameNews.Articles.Domain.Errors;

public class ValidateError(
    string message
) : IError
{
    public string Message { get; } = message;
    public Dictionary<string, object> Metadata { get; } = new();
    public List<IError> Reasons { get; } = [];
}