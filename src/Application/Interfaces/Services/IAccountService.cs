using Application.DTOs;
using Domain.Entities;

namespace Application.Interfaces.Services;

public interface IAccountService
{
    Task<Result> AddAccountsFromCsvAsync(string filePath, long fileId);
    Task<Result<Account>> GetAccountByIdAsync(long accountId);
    Task<Result<List<Account>>> GetAllAccountsAsync();
    Task<Result<List<Account>>> GetAccountsByFileIdAsync(long fileId);
    Task<Result> DeleteAccountsByFileIdAsync(long fileId);
}