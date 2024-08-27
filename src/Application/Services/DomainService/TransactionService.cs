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
        
        var transactions = transactionCsvModels
            .Select(csvModel => csvModel.ToTransaction())
            .ToList();
        
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

    public async Task<Result<List<Transaction>>> GetAllTransactionsAsync()
    {
        try
        {
            var transactions = await _transactionRepository.GetAllTransactions();
            return Result<List<Transaction>>.Ok(transactions);
        }
        catch (Exception ex)
        {
            return Result<List<Transaction>>.Fail($"An error occurred: {ex.Message}");
        }
    }

    public async Task<Result<List<GetTransactionsByAccountIdResponse>>> GetTransactionsByAccountIdAsync(long accountId)
    {
        try
        {
            var result = new List<GetTransactionsByAccountIdResponse>();

            var transactionsSourceAccountId = await _transactionRepository.GetBySourceAccountId(accountId);

            var transactionsDestinationAccountId = await _transactionRepository.GetByDestinationAccountId(accountId);

            var allTransactions = transactionsSourceAccountId.Concat(transactionsDestinationAccountId).ToList();

            var groupedTransactions = allTransactions
                .GroupBy(t => t.SourceAccountId == accountId ? t.DestinationAccountId : t.SourceAccountId)
                .ToList();

            foreach (var group in groupedTransactions)
            {
                var response = new GetTransactionsByAccountIdResponse
                {
                    AccountId = group.Key,
                    TransactionWithSources = group.Select(t => new TransactionCsvModel
                    {
                        TransactionID = t.TransactionId,
                        SourceAcount = t.SourceAccountId,
                        DestiantionAccount = t.DestinationAccountId,
                        Amount = t.Amount,
                        Date = t.Date,
                        Type = t.Type,
                    }).ToList()
                };

                result.Add(response);
            }

            return Result<List<GetTransactionsByAccountIdResponse>>.Ok(result);
        }
        catch (Exception ex)
        {
            return Result<List<GetTransactionsByAccountIdResponse>>.Fail($"An error occurred: {ex.Message}");
        }
    }
}