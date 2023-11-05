namespace GameNews.OAuth.Infrastructure.Api.Exceptions;

public sealed class DiscordApiException : Exception
{
    public DiscordApiException() : base("Discord api exception")
    {
    }

    public DiscordApiException(string error, string errorDescription) : base("Discord api exception")
    {
        Data.Add("Error", error);
        Data.Add("ErrorDescription", errorDescription);
    }
}