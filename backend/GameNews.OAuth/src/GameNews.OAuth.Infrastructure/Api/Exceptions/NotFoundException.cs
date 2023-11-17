namespace GameNews.OAuth.Infrastructure.Api.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException() : base("Not found")
    {
        
    }
}