using Application.DTOs;
using Application.DTOs.Transaction;
using Application.Interfaces;
using Application.Interfaces.Services;
using Application.Mappers;
using Application.Services.SharedService;
using Domain.Entities;
using Application.Interfaces.Repositories;

namespace Application.Services.DomainService;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IFileReaderService _fileReaderService;
    private readonly IAccountRepository _accountRepository;

    public TransactionService(ITransactionRepository transactionRepository, IFileReaderService fileReaderService, IAccountRepository accountRepository)
    {
        _transactionRepository = transactionRepository;
        _fileReaderService = fileReaderService;
        _accountRepository = accountRepository;
    }
    
    private async Task<List<long>> ValidateTransactionCsvModelsAsync(List<TransactionCsvModel> transactionCsvModels)
    {
        var invalidTransactionIds = new List<long>();
        foreach (var transactionCsvModel in transactionCsvModels)
        {
            var sourceAccount = await _accountRepository.GetByIdAsync(transactionCsvModel.SourceAccount);
            var destinationAccount = await _accountRepository.GetByIdAsync(transactionCsvModel.DestinationAccount);
            bool isValidDate = DateOnly.TryParseExact(transactionCsvModel.Date, "MM/dd/yyyy", null, System.Globalization.DateTimeStyles.None, out var date);
            bool isValidTime = TimeOnly.TryParse(transactionCsvModel.Time, out var time);
            if(sourceAccount == null || destinationAccount == null || !isValidDate || !isValidTime)
            {
                invalidTransactionIds.Add(transactionCsvModel.TransactionId);
            }
        }
        return invalidTransactionIds;
    }
    
    public async Task<Result> AddTransactionsFromCsvAsync(string filePath)
    {
        try
        {
            var transactionCsvModels = _fileReaderService.ReadFromFile<TransactionCsvModel>(filePath);
            var invalidTransactionCsvModels = await ValidateTransactionCsvModelsAsync(transactionCsvModels);
            var transactions = transactionCsvModels
                .Where(csvModel => !invalidTransactionCsvModels.Contains(csvModel.TransactionId))
                .Select(csvModel => csvModel.ToTransaction())
                .ToList();
            
            var existingTransactionsIds = await _transactionRepository.GetAllIdsAsync();
            var newTransactions = transactions.Where(t => !existingTransactionsIds.Contains(t.TransactionId)).ToList();
            
            await _transactionRepository.CreateBulkAsync(newTransactions);
            return Result.Ok(invalidTransactionCsvModels.Count == 0
                ? "All transactions were added successfully."
                : $"Some transactions were not added because of invalid data: {string.Join(", ", invalidTransactionCsvModels)}");
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
                        TransactionId = t.TransactionId,
                        SourceAccount = t.SourceAccountId,
                        DestinationAccount = t.DestinationAccountId,
                        Amount = t.Amount,
                        Date = DateOnly.FromDateTime(t.Date).ToString(),
                        Time = TimeOnly.FromDateTime(t.Date).ToString(),
                        Type = t.Type,
                        TrackingId = t.TrackingId
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