﻿namespace GameNews.OAuth.Api.Responses.V1;

public record GetTokenResponse
{
    public string AccessToken { get; init; } = string.Empty;
    public int ExpiresIn { get; init; }
    public string RefreshToken { get; init; } = string.Empty;
};