using GameNews.OAuth.Domain.Interfaces;
using GameNews.OAuth.Domain.Services;
using GameNews.OAuth.Infrastructure.Api;
using GameNews.OAuth.Infrastructure.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GameNews.OAuth.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDiscordApi(this IServiceCollection services, IConfigurationRoot config)
    {
        services.Configure<DiscordApiOptions>(config.GetSection(nameof(DiscordApiOptions)));
        
        services.AddTransient<IDiscordApi, DiscordApi>();

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddTransient<Domain.Services.Interfaces.IOAuth2Service, OAuth2Service>();

        return services;
    }
}