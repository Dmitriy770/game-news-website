using GameNews.MediaStorage.Domain.Interfaces;
using GameNews.MediaStorage.Domain.Services;
using GameNews.MediaStorage.Domain.Services.Interfaces;
using GameNews.MediaStorage.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace GameNews.MediaStorage.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {        
        services.AddTransient<IStorageRepository, StorageRepository>();
        
        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddTransient<IStorageService, StorageService>();

        return services;
    }
}