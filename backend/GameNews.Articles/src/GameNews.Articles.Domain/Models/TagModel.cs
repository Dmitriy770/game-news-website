using FluentResults;
using GameNews.Articles.Domain.Errors;

namespace GameNews.Articles.Domain.Models;

public record TagModel
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public List<ArticleModel> Articles { get; private set; } = [];

    private TagModel()
    {
        
    }
    private TagModel(Guid id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }

    public static Result<TagModel> Create(Guid id, string name, string description)
    {
        name = name.Trim();
        if (name.Length is < 1 or > 20)
        {
            return Result.Fail(new ValidateError("Name length must be from 1 to 20"));
        }

        description = description.Trim();
        if (description.Length is < 1 or > 150)
        {
            return Result.Fail(new ValidateError("Name length must be from 1 to 150"));
        }

        return new TagModel(id, name, description);
    }
}