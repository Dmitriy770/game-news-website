namespace GameNews.OAuth.Infrastructure.Api.Exceptions;

public class GetTokenException : Exception
{
    public GetTokenException() : base("Request token exception")
    {
    }
}