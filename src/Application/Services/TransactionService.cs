using Application.DTOs;
using Application.DTOs.TransactionCsv;
using Application.Interfaces;
using Application.Interfaces.Services;
using Application.Mappers;
using Application.Services.SharedService;
using Domain.Entities;
using Application.DTOs.TransactionCsv;
using Application.Interfaces.Repositories;

namespace Application.Services;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;

    public TransactionService(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task AddTransactionsFromCsvAsync(string filePath)
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
        
        await _transactionRepository.CreateBulkAsync(transactions);
    }

    public async Task<Result<GetAllTransactionsResponse>> GetAllTransactions()
    {
        try
        {
            var transactions = await _transactionRepository.GetAllTransactions();

            if (transactions.Count == 0)
            {
                return Result<GetAllTransactionsResponse>.Fail("No transactions found");
            }
            var response = transactions.ToGetAllTransactionsResponse();
            return Result<GetAllTransactionsResponse>.Ok(response);
        }
        catch (Exception ex)
        {
            return Result<GetAllTransactionsResponse>.Fail($"An error occurred: {ex.Message}");
        }
    }
}