using Application.DTOs;
using Application.DTOs.Transaction;
using Domain.Entities;

namespace Application.Interfaces.Services;

public interface ITransactionService
{
    Task<Result> AddTransactionsFromCsvAsync(string filePath, long fileId);
    Task<Result<List<Transaction>>> GetAllTransactionsAsync();
    Task<Result<List<GetTransactionsByAccountIdResponse>>> GetTransactionsByAccountIdAsync(long accountId);
    Task<Result<List<Transaction>>> GetTransactionsByFileIdAsync(long fileId);
    Task<Result> DeleteTransactionsByFileIdAsync(long fileId);
}