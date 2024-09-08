using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

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

    public async Task<List<Account>> GetAllAccounts()
    {
        return await _dbContext.Accounts.ToListAsync();
    }

    public async Task<List<long>> GetAllIdsAsync()
    {
        return await _dbContext.Accounts
            .Select(a => a.AccountId)
            .ToListAsync();
    }

    public async Task<List<Account>> GetByFileIdAsync(long fileId)
    {
        return await _dbContext.Accounts
            .Where(a => a.FileId == fileId)
            .ToListAsync();
    }

    public async Task DeleteByFileIdAsync(long fileId)
    {
        var accounts = _dbContext.Accounts
            .Where(a => a.FileId == fileId);
        _dbContext.Accounts.RemoveRange(accounts);
        await _dbContext.SaveChangesAsync();
    }
}