using Application.DTOs;
using Application.DTOs.Transaction;
using Domain.Entities;

namespace Application.Interfaces.Services;

public interface ITransactionService
{
    Task<Result> AddTransactionsFromCsvAsync(string filePath);
    Task<Result<PaginatedList<Transaction>>> GetAllTransactionsAsync(int pageNumber, int pageSize);
    Task<Result<List<GetTransactionsByAccountIdResponse>>> GetTransactionsByAccountIdAsync(long accountId);
}