﻿using FluentMigrator.Runner;
using GameNews.Articles.Infrastructure.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GameNews.Articles.IntegrationTests.Fixtures;

public class TestFixture
{
    public TestFixture()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.Development.json")
            .Build();

        var host = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddRepositories(config);
            })
            .Build();
        
        ClearDatabase(host);
        host.MigrateUp();
    }

    private static void ClearDatabase(IHost host)
    {
        using var scope = host.Services.CreateScope();
        var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
        runner.MigrateDown(20230906);
    }
}