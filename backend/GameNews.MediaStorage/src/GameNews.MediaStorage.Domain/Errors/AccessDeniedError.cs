using FluentResults;

namespace GameNews.MediaStorage.Domain.Errors;

public sealed class AccessDeniedError : IError
{
    public string Message { get; } = "Access denied error";
    public Dictionary<string, object> Metadata { get; set; }
    public List<IError> Reasons { get; } = new();
}