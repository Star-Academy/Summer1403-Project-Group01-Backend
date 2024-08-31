using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface ITransactionRepository
{
    Task CreateBulkAsync(List<Transaction> transactions);
    Task<List<Transaction>> GetAllTransactions(int skip, int take);
    Task<List<Transaction>> GetBySourceAccountId(long accountId);
    Task<List<Transaction>> GetByDestinationAccountId(long accountId);
    Task<List<long>> GetAllIdsAsync();
    Task<int> CountAllTransactionsAsync();
}