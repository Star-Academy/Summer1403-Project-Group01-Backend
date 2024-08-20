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

    public async Task AddAccountsFromCsvAsync(string filePath)
    {
        var accountCsvModels = CsvReaderService.ReadFromCsv<AccountCsvModel>(filePath);

        var accounts = accountCsvModels
            .Select(csvModel => csvModel.ToAccount())
            .ToList();
        
        await _accountRepository.CreateBulkAsync(accounts);
    }

    public async Task<Account?> GetAccountByIdAsync(long accountId)
    {
        return await _accountRepository.GetByIdAsync(accountId);
    }
    
    public async Task<Result<List<Transaction>>> GetTransactionsByUserId(long accountId)
    {
        var account = await _accountRepository.GetByIdAsync(accountId);
        
        if (account == null)
        {
            return Result<List<Transaction>>.Fail("Account not found");
        }
        
        var transactions = await _accountRepository.GetTransactionsByAccountId(accountId);
        return Result<List<Transaction>>.Ok(transactions);
    }

    public async Task<Result<GetAllAccountsResponse>> GetAllAccountsAsync()
    {
        try
        {
            var accounts = await _accountRepository.GetAllAccounts();
            var response = accounts.ToGetAllAccountsResponse();
            return Result<GetAllAccountsResponse>.Ok(response);
        }
        catch (Exception ex)
        {
            return Result<GetAllAccountsResponse>.Fail($"An error occurred: {ex.Message}");
        }
    }
}