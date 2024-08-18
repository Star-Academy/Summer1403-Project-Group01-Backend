using Application.DTOs;
using Domain.Entities;

namespace Application.Interfaces.Services;

public interface IAccountService
{
    Task AddAccountsFromCsvAsync(string filePath);
    Task<Account?> GetAccountByIdAsync(long accountId);
    Task<Result<List<Transaction>>> GetTransactionsByUserId(long accountId);
}