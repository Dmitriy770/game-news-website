using FluentResults;
using GameNews.Articles.Domain.Errors;

namespace GameNews.Articles.Domain.Models;

public record ArticleModel
{
    public Guid Id { get; private set; }
    public string Title { get; private set;}
    public Guid? PreviewMediaId { get; private set;}
    public string? PreviewText { get; private set;}
    public List<TagModel> Tags { get; private set; } = [];
    public DateTime CreationDate { get; private set;}
    public string AuthorId { get; private set;}
    public bool IsVisible { get; private set; }
    public string Content { get; private set;}

    public void Hide()
    {
        IsVisible = false;
    }

    public void Show()
    {
        IsVisible = true;
    }

    private ArticleModel()
    {
        
    }
    private ArticleModel(
        Guid id,
        string title,
        Guid? previewMediaId,
        string? previewText,
        List<TagModel> tags,
        DateTime creationDate,
        string authorId,
        bool isVisible,
        string content)
    {
        Id = id;
        Title = title;
        PreviewMediaId = previewMediaId;
        PreviewText = previewText;
        Tags = tags;
        CreationDate = creationDate;
        AuthorId = authorId;
        IsVisible = isVisible;
        Content = content;
    }

    public static Result<ArticleModel> Create(
        Guid id,
        string title,
        Guid? previewMediaId,
        string? previewText,
        List<TagModel> tags,
        DateTime creationDate,
        string authorId,
        bool isVisible,
        string content)
    {
        title = title.Trim();
        if (title.Length is < 1 or > 150)
        {
            return Result.Fail(new ValidateError("Title length must be from 1 to 150"));
        }

        previewText = previewText?.Trim();
        if (previewText?.Length is < 1 or > 800)
        {
            return Result.Fail(new ValidateError("Preview text length must be from 1 to 150"));
        }

        content = content.Trim();

        return new ArticleModel(
            id,
            title,
            previewMediaId,
            previewText,
            tags,
            creationDate,
            authorId,
            isVisible,
            content
        );
    }

    public static Result<ArticleModel> Create(
        Guid id,
        DateTime creationDate,
        string authorId
    )
    {
        return new ArticleModel(
            id,
            "New article",
            null,
            null,
            [],
            creationDate,
            authorId,
            false,
            ""
        );
    }
}