using Application.DTOs.Account;
using Domain.Entities;

namespace Application.Mappers;

public static class AccountMapper
{
    public static GetAllAccountsResponse ToGetAllAccountsResponse(this List<Account> accounts)
    {
        return new GetAllAccountsResponse
        {
            Accounts = accounts.Select(account => new AccountCsvModel
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
            }).ToList()
        };
    }
}