using FluentResults;

namespace GameNews.Articles.Domain.Errors;

public class TagNotFoundError : IError
{
    public string Message { get; } = "Tag not found";
    public Dictionary<string, object> Metadata { get; } = new();
    public List<IError> Reasons { get; } = [];
}