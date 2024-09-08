using Application.DTOs.Account;
using Domain.Entities;

namespace Application.Mappers;

public static class AccountMapper
{
    public static Account ToAccount(this AccountCsvModel csvModel, long fileId)
    {
        return new Account
        {
            AccountId = csvModel.AccountId,
            CardId = csvModel.CardId,
            Iban = csvModel.Iban,
            AccountType = csvModel.AccountType,
            BranchTelephone = csvModel.BranchTelephone,
            BranchAddress = csvModel.BranchAddress,
            BranchName = csvModel.BranchName,
            OwnerName = csvModel.OwnerName,
            OwnerLastName = csvModel.OwnerLastName,
            OwnerId = csvModel.OwnerId,
            FileId = fileId
        };
    }
}