namespace GameNews.OAuth.Infrastructure.Api.Responses;

public record GetUserResponse(
    Application Application,
    string[] Scopes,
    string Expires,
    User User 
);