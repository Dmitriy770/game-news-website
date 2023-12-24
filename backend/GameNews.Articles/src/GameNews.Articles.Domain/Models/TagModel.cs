using FluentResults;
using GameNews.Articles.Domain.Errors;

namespace GameNews.Articles.Domain.Models;

public record TagModel
{
    public Guid Id { get; }
    public string Name { get; }
    public string Description { get; }
    
    public List<ArticleModel> Articles { get; set; }

    public TagModel()
    {
        
    }
    public TagModel(Guid id, string name, string description)
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