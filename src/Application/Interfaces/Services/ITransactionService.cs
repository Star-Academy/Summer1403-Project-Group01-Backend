using Application.DTOs;

namespace Application.Interfaces.Services;

public interface ITransactionService
{
    Task AddTransactionsFromCsvAsync(string filePath);
}