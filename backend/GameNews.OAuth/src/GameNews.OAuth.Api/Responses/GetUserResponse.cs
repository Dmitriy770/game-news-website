namespace GameNews.OAuth.Api.Responses;

public record GetUserResponse(
    string Name,
    string AvatarUrl,
    string Role
);
