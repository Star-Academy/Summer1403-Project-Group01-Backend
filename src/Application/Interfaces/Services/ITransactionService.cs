using Application.DTOs;
using Domain.Entities;

namespace Application.Interfaces.Services;

public interface ITransactionService
{
    Task<Result> AddTransactionsFromCsvAsync(string filePath);
    Task<Result<List<Transaction>>> GetAllTransactionsAsync();
    Task<List<Transaction>> GetTransactionsByAccountIdAsync(long accountId);
}