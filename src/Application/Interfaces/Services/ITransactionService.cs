using Application.DTOs;
using Application.DTOs.Transaction;

namespace Application.Interfaces.Services;

public interface ITransactionService
{
    Task AddTransactionsFromCsvAsync(string filePath);
    Task<Result<GetAllTransactionsResponse>> GetAllTransactionsAsync();
}