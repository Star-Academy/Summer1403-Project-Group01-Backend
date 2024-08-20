using Application.DTOs;
using Application.DTOs.Transaction;
using Application.Interfaces.Services;
using Application.Mappers;
using Application.Services.SharedService;
using Domain.Entities;
using Application.Interfaces.Repositories;

namespace Application.Services.DomainService;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;

    public TransactionService(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task<Result> AddTransactionsFromCsvAsync(string filePath)
    {
        var transactionCsvModels = CsvReaderService.ReadFromCsv<TransactionCsvModel>(filePath);
        
        var transactions = transactionCsvModels.Select(csvModel => new Transaction
        {
            TransactionId = csvModel.TransactionID,
            SourceAccountId = csvModel.SourceAcount,
            DestinationAccountId = csvModel.DestiantionAccount,
            Amount = csvModel.Amount,
            Date = csvModel.Date,
            Type = csvModel.Type
        }).ToList();
        try
        {
            await _transactionRepository.CreateBulkAsync(transactions);
            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail($"An error occurred: {ex.Message}");
        }
    }

    public async Task<Result<GetAllTransactionsResponse>> GetAllTransactionsAsync()
    {
        try
        {
            var transactions = await _transactionRepository.GetAllTransactions();
            var response = transactions.ToGetAllTransactionsResponse();
            return Result<GetAllTransactionsResponse>.Ok(response);
        }
        catch (Exception ex)
        {
            return Result<GetAllTransactionsResponse>.Fail($"An error occurred: {ex.Message}");
        }
    }
}