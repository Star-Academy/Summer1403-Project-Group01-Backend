using Domain.Entities;

namespace Application.Interfaces;

public interface IAccountRepository
{
    Task CreateBulkAsync(List<Account> accounts);
    Task<Account?> GetByIdAsync(long accountId);
    Task<List<Transaction>> GetTransactionsByAccountId(long accountId);
}