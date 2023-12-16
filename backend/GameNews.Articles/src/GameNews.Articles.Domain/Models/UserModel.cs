using FluentResults;
using GameNews.Articles.Domain.Models.ValueTypes;

namespace GameNews.Articles.Domain.Models;

public record UserModel
{
    public string Id { get; }
    public string Name { get; }
    public RoleType Role { get; }

    private UserModel(string id, string name, RoleType role)
    {
        Id = id;
        Name = name;
        Role = role;
    }

    public static Result<UserModel> Create(string id, string name, RoleType role)
    {
        return new UserModel(id, name, role);
    }
}