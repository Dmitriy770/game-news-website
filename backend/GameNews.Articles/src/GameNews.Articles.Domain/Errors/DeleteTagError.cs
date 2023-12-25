using FluentResults;

namespace GameNews.Articles.Domain.Errors;

public sealed class DeleteTagError : IError
{
    public string Message { get; } = "Delete tag error";
    public Dictionary<string, object> Metadata { get; } = new();
    public List<IError> Reasons { get; } = [];
}