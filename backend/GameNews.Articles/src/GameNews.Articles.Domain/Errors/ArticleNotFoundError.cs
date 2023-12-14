using FluentResults;

namespace GameNews.Articles.Domain.Errors;

public class ArticleNotFoundError : IError
{
    public string Message { get; } = "Article not found";
    public Dictionary<string, object> Metadata { get; } = new();
    public List<IError> Reasons { get; } = [];
}