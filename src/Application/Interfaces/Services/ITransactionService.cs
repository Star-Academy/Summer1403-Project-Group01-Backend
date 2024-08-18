using Application.DTOs;
using Web.DTOs.Transaction;

namespace Application.Interfaces.Services;

public interface ITransactionService
{
    Task AddTransactionsFromCsvAsync(string filePath);
    Task<Result<GetAllTransactionsResponse>> GetAllTransactions();
}