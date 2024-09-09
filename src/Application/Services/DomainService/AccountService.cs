using Application.DTOs;
using Application.DTOs.Account;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Mappers;
using Application.Services.SharedService;
using Domain.Entities;

namespace Application.Services.DomainService;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IFileReaderService _fileReaderService;
    private readonly IFileIdRepository _fileIdRepository;

    public AccountService(IAccountRepository accountRepository, IFileReaderService fileReaderService, IFileIdRepository fileIdRepository)
    {
        _accountRepository = accountRepository;
        _fileReaderService = fileReaderService;
        _fileIdRepository = fileIdRepository;
    }

    public async Task<Result> AddAccountsFromCsvAsync(string filePath, long fileId)
    {
        try
        {
            var accountCsvModels = _fileReaderService.ReadFromFile<AccountCsvModel>(filePath);

            var accounts = accountCsvModels
                .Select(csvModel => csvModel.ToAccount(fileId))
                .ToList();
        
            var existingAccountIds = await _accountRepository.GetAllIdsAsync();
            var newAccounts = accounts.Where(a => !existingAccountIds.Contains(a.AccountId)).ToList();
            
            if(newAccounts.Count == 0)
            {
                return Result.Fail(ErrorCode.BadRequest, "No new accounts to add");
            }
            
            var fileAlreadyExists = await _fileIdRepository.IdExistsAsync(fileId);
            if (fileAlreadyExists)
            {
                return Result.Fail(ErrorCode.BadRequest, "File-Id already exists");
            }
            await _fileIdRepository.AddAsync(new FileId { Id = fileId });
            await _accountRepository.CreateBulkAsync(newAccounts);
            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail(ErrorCode.InternalServerError, $"An unexpected error occurred: {ex.Message}");
        }
    }

    public async Task<Result<Account>> GetAccountByIdAsync(long accountId)
    {
        try
        {
            var account = await _accountRepository.GetByIdAsync(accountId);
            if (account == null)
            {
                return Result<Account>.Fail(ErrorCode.NotFound, "Account not found");
            }
        
            return Result<Account>.Ok(account);
        }
        catch (Exception ex)
        {
            return Result<Account>.Fail(ErrorCode.InternalServerError, $"An unexpected error occurred: {ex.Message}");
        }
    }

    public async Task<Result<List<Account>>> GetAllAccountsAsync()
    {
        try
        {
            var accounts = await _accountRepository.GetAllAccounts();
            return Result<List<Account>>.Ok(accounts);
        }
        catch (Exception ex)
        {
            return Result<List<Account>>.Fail(ErrorCode.InternalServerError, $"An unexpected error occurred: {ex.Message}");
        }
    }
    
    public async Task<Result<List<Account>>> GetAccountsByFileIdAsync(long fileId)
    {
        try
        {
            if (!await _fileIdRepository.IdExistsAsync(fileId))
            {
                return Result<List<Account>>.Fail(ErrorCode.BadRequest, "File-Id not found");
            }
            var accounts = await _accountRepository.GetByFileIdAsync(fileId);
            return Result<List<Account>>.Ok(accounts);
        }
        catch (Exception ex)
        {
            return Result<List<Account>>.Fail(ErrorCode.InternalServerError, $"An unexpected error occurred: {ex.Message}");
        }
    }

    public async Task<Result> DeleteAccountsByFileIdAsync(long fileId)
    {
        try
        {
            if (!await _fileIdRepository.IdExistsAsync(fileId))
            {
                return Result<List<Account>>.Fail(ErrorCode.BadRequest, "File-Id not found");
            }
            await _accountRepository.DeleteByFileIdAsync(fileId);
            await _fileIdRepository.DeleteByIdAsync(fileId);
            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail(ErrorCode.InternalServerError, $"An unexpected error occurred: {ex.Message}");
        }
    }
}