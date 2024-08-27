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
        // var accountCsvModels = CsvReaderService.ReadFromCsv(filePath);
        //
        // var accounts = accountCsvModels
        //     .Select(csvModel => csvModel.ToAccount())
        //     .ToList();
        // try
        // {
        //     await _accountRepository.CreateBulkAsync(accounts);
        //     return Result.Ok();
        // }
        // catch (Exception ex)
        // {
        //     return Result.Fail($"An error occurred: {ex.Message}");
        // }
        throw new NotImplementedException();
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