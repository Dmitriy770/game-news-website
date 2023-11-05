namespace GameNews.OAuth.Infrastructure.Api.Responses;

public record ErrorResponse(
    string Error,
    string ErrorDescription
);