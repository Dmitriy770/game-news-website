﻿using GameNews.Articles.Domain.Interfaces;
using GameNews.Articles.Domain.Services;
using GameNews.Articles.Domain.Services.Interfaces;
using GameNews.Articles.Infrastructure.Options;
using GameNews.Articles.Infrastructure.Repositories.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace GameNews.Articles.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services, IConfigurationRoot config)
    {
        services.Configure<PostgresOptions>(config.GetSection(nameof(PostgresOptions)));

        var psConfig = services.BuildServiceProvider().GetService<IOptions<PostgresOptions>>()!.Value;

        services.AddDbContext<ArticleRepositoryContext>(options =>
        {
            options.UseNpgsql(
                $"Host={psConfig.Host};Port={psConfig.Port};Database={psConfig.Database};Username={psConfig.User};Password={psConfig.Password}");
        });
        services.AddTransient<IArticleRepository, ArticleRepository>();

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddTransient<ITagService, TagService>();
        return services;
    }
}