namespace GameNews.MediaStorage.Infrastructure.Options;

public record MongoOptions
{
    public string User { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;

    public void Deconstruct(out string user, out string password)
        => (user, password) = (User, Password);
}