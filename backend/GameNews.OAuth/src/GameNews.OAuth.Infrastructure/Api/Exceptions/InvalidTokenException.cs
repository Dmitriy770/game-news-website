namespace GameNews.OAuth.Infrastructure.Api.Exceptions;

public class InvalidTokenException : Exception
{
    public InvalidTokenException() : base("Invalid token exception")
    {
    }
}