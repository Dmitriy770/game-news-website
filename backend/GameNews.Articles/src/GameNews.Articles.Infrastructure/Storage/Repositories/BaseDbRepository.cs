using System.Transactions;
using GameNews.Articles.Infrastructure.Storage.Repositories.Interfaces;
using Npgsql;

namespace GameNews.Articles.Infrastructure.Storage.Repositories;

public abstract class BaseDbRepository : IDbRepository
{
    private readonly StorageOptions _dbSettings;

    protected BaseDbRepository(StorageOptions dbSettings)
    {
        _dbSettings = dbSettings;
    }

    protected async Task<NpgsqlConnection> GetAndOpenConnection()
    {
        var connectionStringBuilder = new NpgsqlConnectionStringBuilder
        {
            Host = _dbSettings.Host,
            Port = _dbSettings.Port,
            Username = _dbSettings.User,
            Password = _dbSettings.Password,
            Database = _dbSettings.Database
        };

        var connection = new NpgsqlConnection(connectionStringBuilder.ConnectionString);
        await connection.OpenAsync();
        await connection.ReloadTypesAsync();

        return connection;
    }
    
    public TransactionScope CreateTransactionScope(IsolationLevel level = IsolationLevel.ReadCommitted)
    {
        return new TransactionScope(
            TransactionScopeOption.Required,
            new TransactionOptions
            {
                IsolationLevel = level,
                Timeout = TimeSpan.FromSeconds(5)
            },
            TransactionScopeAsyncFlowOption.Enabled);
    }
}