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

    public AccountService(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<Result> AddAccountsFromCsvAsync(string filePath)
    {
        var accountCsvModels = CsvReaderService.ReadFromCsv<AccountCsvModel>(filePath);

        var accounts = accountCsvModels
            .Select(csvModel => csvModel.ToAccount())
            .ToList();
        try
        {
            var existingAccountIds = await _accountRepository.GetAllIdsAsync();
            var newAccounts = accounts.Where(a => !existingAccountIds.Contains(a.AccountId)).ToList();

            await _accountRepository.CreateBulkAsync(newAccounts);
            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail($"An error occurred: {ex.Message}");
        }
    }

    public async Task<Account?> GetAccountByIdAsync(long accountId)
    {
        return await _accountRepository.GetByIdAsync(accountId);
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
            return Result<List<Account>>.Fail($"An error occurred: {ex.Message}");
        }
    }
}