using Application.DTOs;
using Application.DTOs.TransactionCsv;

namespace Application.Interfaces.Services;

public interface ITransactionService
{
    Task AddTransactionsFromCsvAsync(string filePath);
    Task<Result<GetAllTransactionsResponse>> GetAllTransactions();
}