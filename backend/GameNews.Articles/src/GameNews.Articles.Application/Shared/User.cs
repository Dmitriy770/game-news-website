namespace GameNews.Articles.Application.Shared;

public record User(
    string Id,
    string Name,
    string Role
)
{
    public const string AdministratorRole = "Administrator";
    public const string AuthorRole = "Author";
    public const string UserRole = "User";
}