namespace GameNews.OAuth.Infrastructure.Api.Exceptions;

public class InvalidCodeException : Exception
{
    public InvalidCodeException() : base("Invalid coed exception")
    {
    }
}