using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface ITransactionRepository
{
    Task CreateBulkAsync(List<Transaction> transactions);
    Task<List<Transaction>> GetAllTransactions();
}