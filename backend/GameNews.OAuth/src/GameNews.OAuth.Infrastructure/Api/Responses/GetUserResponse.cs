using GameNews.OAuth.Infrastructure.Api.Entities;

namespace GameNews.OAuth.Infrastructure.Api.Responses;

public record GetUserResponse(
    string[] Scopes,
    string Expires,
    UserEntity User 
);