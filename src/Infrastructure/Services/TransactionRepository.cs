using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Services;

public class TransactionRepository : ITransactionRepository
{
    private readonly ApplicationDbContext _dbContext;
    public TransactionRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateBulkAsync(List<Transaction> transactions)
    {
        await _dbContext.Transactions.AddRangeAsync(transactions);
        await _dbContext.SaveChangesAsync();
    }
}