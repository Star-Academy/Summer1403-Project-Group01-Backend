using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class AccountRepository : IAccountRepository
{
    private readonly ApplicationDbContext _dbContext;

    public AccountRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateBulkAsync(List<Account> accounts)
    {
        await _dbContext.Accounts.AddRangeAsync(accounts);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Account?> GetByIdAsync(long accountId)
    {
        return await _dbContext.Accounts.FindAsync(accountId);
    }

    public async Task<List<Transaction>> GetTransactionsByAccountId(long accountId)
    {
        var account = await _dbContext.Accounts
            .Include(a => a.SourceTransactions)
            .FirstOrDefaultAsync(a => a.AccountId == accountId);
        
        if (account == null)
        {
            return new List<Transaction>();
        }

        return account.SourceTransactions.ToList();
    }
}