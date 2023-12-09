using FluentResults;

namespace GameNews.MediaStorage.Domain.Errors;

public sealed class MediaNotFoundError : IError
{
    public string Message { get; } = "File not found";
    public Dictionary<string, object> Metadata { get; set; }
    public List<IError> Reasons { get; } = new();
}