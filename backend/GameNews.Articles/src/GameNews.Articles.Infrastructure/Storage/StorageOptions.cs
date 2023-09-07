namespace GameNews.Articles.Infrastructure.Storage;

public record StorageOptions
{
    public string Host { get; init; } = string.Empty;
    public int Port { get; init; }
    public string User { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string Database { get; init; } = string.Empty;
}