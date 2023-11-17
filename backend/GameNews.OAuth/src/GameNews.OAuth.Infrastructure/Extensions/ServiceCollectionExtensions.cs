using GameNews.OAuth.Domain.Interfaces;
using GameNews.OAuth.Domain.Services;
using GameNews.OAuth.Domain.Services.Interfaces;
using GameNews.OAuth.Infrastructure.Api;
using GameNews.OAuth.Infrastructure.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GameNews.OAuth.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfigurationRoot config)
    {
        services.Configure<DiscordApiOptions>(config.GetSection(nameof(DiscordApiOptions)));
        
        services.AddHttpClient("discordApi",client =>
        {
            client.BaseAddress = new Uri("https://discord.com/api/v10/");
        });
        
        services.AddTransient<IDiscordClient, DiscordClient>();
        
        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddTransient<IAuthService, AuthService>();

        return services;
    }
}