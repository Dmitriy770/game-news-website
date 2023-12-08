namespace GameNews.Articles.Domain.Models;

public record UserModel
{
    public string Id { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public RoleModel Role { get; init; } = RoleModel.Anonymous;
};