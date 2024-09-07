using Application.DTOs;
using Application.DTOs.Account;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Mappers;
using Application.Services.SharedService;
using Domain.Entities;

namespace Application.Services.DomainService;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly ICsvReaderService _csvReaderService;

    public AccountService(IAccountRepository accountRepository, ICsvReaderService csvReaderService)
    {
        _accountRepository = accountRepository;
        _csvReaderService = csvReaderService;
    }

    public async Task<Result> AddAccountsFromCsvAsync(string filePath)
    {
        try
        {
            var accountCsvModels = _csvReaderService.ReadFromCsv<AccountCsvModel>(filePath);

            var accounts = accountCsvModels
                .Select(csvModel => csvModel.ToAccount())
                .ToList();
        
            var existingAccountIds = await _accountRepository.GetAllIdsAsync();
            var newAccounts = accounts.Where(a => !existingAccountIds.Contains(a.AccountId)).ToList();

            await _accountRepository.CreateBulkAsync(newAccounts);
            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail($"An unexpected error occurred: {ex.Message}");
        }
    }

    public async Task<Result<Account>> GetAccountByIdAsync(long accountId)
    {
        try
        {
            var account = await _accountRepository.GetByIdAsync(accountId);
            if (account == null)
            {
                return Result<Account>.Fail("Account not found");
            }
        
            return Result<Account>.Ok(account);
        }
        catch (Exception ex)
        {
            return Result<Account>.Fail($"An unexpected error occurred: {ex.Message}");
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
            return Result<List<Account>>.Fail($"An unexpected error occurred: {ex.Message}");
        }
    }
}