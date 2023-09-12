using GameNews.Articles.Domain.Interfaces;
using GameNews.Articles.Infrastructure.Storage;
using GameNews.Articles.Infrastructure.Storage.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GameNews.Articles.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services, IConfigurationRoot config)
    {
        services.Configure<StorageOptions>(config.GetSection(nameof(StorageOptions)));

        Postgres.MapCompositeTypes();
        Postgres.AddMigrations(services);

        services.AddTransient<IArticleRepository, ArticleRepository>();
        
        return services;
    }
}