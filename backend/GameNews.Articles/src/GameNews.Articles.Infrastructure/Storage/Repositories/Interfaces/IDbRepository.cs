using System.Transactions;

namespace GameNews.Articles.Infrastructure.Storage.Repositories.Interfaces;

public interface IDbRepository
{
    TransactionScope CreateTransactionScope(IsolationLevel level = IsolationLevel.ReadCommitted);
}