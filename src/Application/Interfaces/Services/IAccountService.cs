using Application.DTOs;
using Application.DTOs.Account;
using Domain.Entities;

namespace Application.Interfaces.Services;

public interface IAccountService
{
    Task<Result> AddAccountsFromCsvAsync(string filePath);
    Task<Account?> GetAccountByIdAsync(long accountId);
    Task<Result<List<Transaction>>> GetTransactionsByUserId(long accountId);
    Task<Result<GetAllAccountsResponse>> GetAllAccountsAsync();
}