using CsvHelper.Configuration.Attributes;

namespace Application.DTOs.Account;

public class AccountCsvModel
{
    [Name("AccountId", "AccountID")]
    public long AccountId { get; set; }
    [Name("CardId", "CardID")]
    public long CardId { get; set; }
    [Name("Iban", "IBAN", "Sheba")]
    public string Iban { get; set; } = string.Empty;
    [Name("AccountType")]
    public string AccountType { get; set; } = string.Empty;
    [Name("BranchTelephone")]
    public string BranchTelephone { get; set; } = string.Empty;
    [Name("BranchAddress", "BranchAdress")]
    public string BranchAddress { get; set; } = string.Empty;
    [Name("BranchName")]
    public string BranchName { get; set; } = string.Empty;
    [Name("OwnerName")]
    public string OwnerName { get; set; } = string.Empty;
    [Name("OwnerLastName", "OwnerFamilyName")]
    public string OwnerLastName { get; set; } = string.Empty;
    [Name("OwnerId", "OwnerID")]
    public long OwnerId { get; set; }
}