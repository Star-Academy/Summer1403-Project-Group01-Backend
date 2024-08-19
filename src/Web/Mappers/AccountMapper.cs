using Application.DTOs.AccountCsv;
using Domain.Entities;
using Web.DTOs.Account;

namespace Web.Mappers;

public static class AccountMapper
{
    public static AccountDto ToAccountDto(this Account account)
    {
        return new AccountDto
        {
            AccountId = account.AccountId,
            CardId = account.CardId,
            Iban = account.Iban,
            AccountType = account.AccountType,
            BranchTelephone = account.BranchTelephone,
            BranchAddress = account.BranchAddress,
            BranchName = account.BranchName,
            OwnerName = account.OwnerName,
            OwnerLastName = account.OwnerLastName,
            OwnerId = account.OwnerId
        };
    }
    
    public static List<AccountDto> ToGotAllAccountsDto(this GetAllAccountsResponse response)
    {
        return response.Accounts.Select(account => new AccountDto
        {
            AccountId = account.AccountID,
            CardId = account.CardID,
            Iban = account.IBAN,
            AccountType = account.AccountType,
            BranchTelephone = account.BranchTelephone,
            BranchAddress = account.BranchAdress,
            BranchName = account.BranchName,
            OwnerName = account.OwnerName,
            OwnerLastName = account.OwnerLastName,
            OwnerId = account.OwnerID
        }).ToList();
    }
}