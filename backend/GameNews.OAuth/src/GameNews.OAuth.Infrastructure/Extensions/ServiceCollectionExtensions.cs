using GameNews.OAuth.Domain.Interfaces;
using GameNews.OAuth.Domain.Services;
using GameNews.OAuth.Domain.Services.Interfaces;
using GameNews.OAuth.Infrastructure.Api.Discord;
using Microsoft.Extensions.DependencyInjection;

namespace GameNews.OAuth.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDiscordApi(this IServiceCollection services)
    {
        services.AddTransient<IOAuthApi, OAuthApi>();

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddTransient<IOAuthService, OAuthService>();

        return services;
    }
}