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
    private readonly IFileIdRepository _fileIdRepository;
    public TransactionService(ITransactionRepository transactionRepository,
        IFileReaderService fileReaderService,
        IAccountRepository accountRepository,
        IFileIdRepository fileIdRepository)
    {
        _transactionRepository = transactionRepository;
        _fileReaderService = fileReaderService;
        _accountRepository = accountRepository;
        _fileIdRepository = fileIdRepository;
    }
    
    private async Task<List<long>> ValidateTransactionCsvModelsAsync(List<TransactionCsvModel> transactionCsvModels, long fileId)
    {
        var invalidTransactionIds = new List<long>();
        foreach (var transactionCsvModel in transactionCsvModels)
        {
            var sourceAccount = await _accountRepository.GetByIdAsync(transactionCsvModel.SourceAccount);
            var destinationAccount = await _accountRepository.GetByIdAsync(transactionCsvModel.DestinationAccount);
            bool isValidSourceAccount = (sourceAccount != null) && (sourceAccount.FileId == fileId);
            bool isValidDestinationAccount = (destinationAccount != null) && (destinationAccount.FileId == fileId);
            bool isValidDate = DateOnly.TryParseExact(transactionCsvModel.Date, "MM/dd/yyyy", null, System.Globalization.DateTimeStyles.None, out var date);
            bool isValidTime = TimeOnly.TryParse(transactionCsvModel.Time, out var time);
            if(!isValidSourceAccount || !isValidDestinationAccount || !isValidDate || !isValidTime)
            {
                invalidTransactionIds.Add(transactionCsvModel.TransactionId);
            }
        }
        return invalidTransactionIds;
    }
    
    public async Task<Result> AddTransactionsFromCsvAsync(string filePath, long fileId)
    {
        try
        {
            var transactionCsvModels = _fileReaderService.ReadFromFile<TransactionCsvModel>(filePath);
            var invalidTransactionCsvModels = await ValidateTransactionCsvModelsAsync(transactionCsvModels, fileId);
            var transactions = transactionCsvModels
                .Where(csvModel => !invalidTransactionCsvModels.Contains(csvModel.TransactionId))
                .Select(csvModel => csvModel.ToTransaction(fileId))
                .ToList();
            
            var existingTransactionsIds = await _transactionRepository.GetAllIdsAsync();
            var newTransactions = transactions.Where(t => !existingTransactionsIds.Contains(t.TransactionId)).ToList();
            
            var fileAlreadyExists = await _fileIdRepository.IdExistsAsync(fileId);
            if (!fileAlreadyExists)
            {
                return Result.Fail(ErrorCode.BadRequest, "File-Id do not exist");
            }
            await _transactionRepository.CreateBulkAsync(newTransactions);
            return Result.Ok(invalidTransactionCsvModels.Count == 0
                ? "All transactions were added successfully."
                : $"{invalidTransactionCsvModels.Count} transactions were not added because of invalid data: {string.Join(", ", invalidTransactionCsvModels)}");
        }
        catch (Exception ex)
        {
            return Result.Fail(ErrorCode.InternalServerError, $"An error occurred: {ex.Message}");
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
            return Result<List<Transaction>>.Fail(ErrorCode.InternalServerError, $"An error occurred: {ex.Message}");
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
            return Result<List<GetTransactionsByAccountIdResponse>>.Fail(ErrorCode.InternalServerError, $"An error occurred: {ex.Message}");
        }
    }

    public async Task<Result<List<Transaction>>> GetTransactionsByFileIdAsync(long fileId)
    {
        try
        {
            if (!await _fileIdRepository.IdExistsAsync(fileId))
            {
                return Result<List<Transaction>>.Fail(ErrorCode.BadRequest, "File-Id not found");
            }
            var transactions = await _transactionRepository.GetByFileIdAsync(fileId);
            return Result<List<Transaction>>.Ok(transactions);
        }
        catch (Exception ex)
        {
            return Result<List<Transaction>>.Fail(ErrorCode.InternalServerError, $"An error occurred: {ex.Message}");
        }
    }

    public async Task<Result> DeleteTransactionsByFileIdAsync(long fileId)
    {
        try
        {
            if (!await _fileIdRepository.IdExistsAsync(fileId))
            {
                return Result.Fail(ErrorCode.BadRequest, "File-Id not found");
            }
            await _transactionRepository.DeleteByFileIdAsync(fileId);
            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail(ErrorCode.InternalServerError, $"An error occurred: {ex.Message}");
        }
    }
}