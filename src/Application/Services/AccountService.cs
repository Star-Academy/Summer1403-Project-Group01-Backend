using Application.DTOs.AccountCsv;
using Application.Interfaces;
using Application.Interfaces.Services;
using Application.Services.SharedService;
using Domain.Entities;

namespace Application.Services;

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

        var accounts = accountCsvModels.Select(csvModel => new Account
        {
            AccountId = csvModel.AccountID,
            CardId = csvModel.CardID,
            Iban = csvModel.IBAN,
            AccountType = csvModel.AccountType,
            BranchTelephone = csvModel.BranchTelephone,
            BranchAddress = csvModel.BranchAdress,
            BranchName = csvModel.BranchName,
            OwnerName = csvModel.OwnerName,
            OwnerLastName = csvModel.OwnerLastName,
            OwnerId = csvModel.OwnerID
        }).ToList();
        
        await _accountRepository.CreateBulkAsync(accounts);
    }

    public async Task<Account?> GetAccountByIdAsync(long accountId)
    {
        return await _accountRepository.GetByIdAsync(accountId);
    }
}