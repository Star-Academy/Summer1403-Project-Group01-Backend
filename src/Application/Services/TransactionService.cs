using Application.DTOs;
using Application.DTOs.TransactionCsv;
using Application.Interfaces;
using Application.Interfaces.Services;
using Application.Services.SharedService;
using Domain.Entities;

namespace Application.Services;

public class TransactionService : ITransactionService
{
    public readonly ITransactionRepository _transactionRepository;

    public TransactionService(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task AddTransactionsFromCsvAsync(string filePath)
    {
        var transactionCsvModels = CsvReaderService.ReadFromCsv<TransactionCsvModel>(filePath);
        
        var transactions = transactionCsvModels.Select(csvModel => new Transaction
        {
            SourceAccountId = csvModel.SourceAccount,
            DestinationAccountId = csvModel.DestinationAccount,
            Amount = csvModel.Amount,
            Date = csvModel.Date,
            Type = csvModel.Type
        }).ToList();
        
        await _transactionRepository.CreateBulkAsync(transactions);
    }
}