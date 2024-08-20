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
                AccountID = account.AccountId,
                CardID = account.CardId,
                IBAN = account.Iban,
                AccountType = account.AccountType,
                BranchTelephone = account.BranchTelephone,
                BranchAdress = account.BranchAddress,
                BranchName = account.BranchName,
                OwnerName = account.OwnerName,
                OwnerLastName = account.OwnerLastName,
                OwnerID = account.OwnerId
            }).ToList()
        };
    }

    public static Account ToAccount(this AccountCsvModel csvModel)
    {
        return new Account
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
        };
    }
}