using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Npgsql;
using Npgsql.NameTranslation;

namespace GameNews.Articles.Infrastructure.Storage;

public static class Postgres
{
    private static readonly INpgsqlNameTranslator Translator = new NpgsqlSnakeCaseNameTranslator();

    public static void MapCompositeTypes()
    {
        var mapper = new NpgsqlDataSourceBuilder();

        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
    }

    public static void AddMigrations(IServiceCollection services)
    {
        services.AddFluentMigratorCore()
            .ConfigureRunner(rb => rb.AddPostgres()
                .WithGlobalConnectionString(s =>
                {
                    var storageSettings = s.GetRequiredService<IOptions<StorageOptions>>().Value;

                    var connectionStringBuilder = new NpgsqlConnectionStringBuilder
                    {
                        Host = storageSettings.Host,
                        Port = storageSettings.Port,
                        Username = storageSettings.User,
                        Password = storageSettings.Password,
                        Database = storageSettings.Database,
                    };

                    return connectionStringBuilder.ConnectionString;
                })
                .ScanIn(typeof(Postgres).Assembly).For.Migrations()
            )
            .AddLogging(lb => lb.AddFluentMigratorConsole());
    }
}