namespace GameNews.OAuth.Infrastructure.Api.Exceptions;

public class DiscordApiException : Exception
{
    public DiscordApiException() : base("Discord api exception")
    {
    }
}