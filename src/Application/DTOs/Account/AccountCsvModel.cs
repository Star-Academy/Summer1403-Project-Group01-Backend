namespace Application.DTOs.Account;

public class AccountCsvModel
{
    public long AccountId { get; set; }
    public long CardId { get; set; }
    public string Iban { get; set; } = string.Empty;
    public string AccountType { get; set; } = string.Empty;
    public string BranchTelephone { get; set; } = string.Empty;
    public string BranchAddress { get; set; } = string.Empty;
    public string BranchName { get; set; } = string.Empty;
    public string OwnerName { get; set; } = string.Empty;
    public string OwnerLastName { get; set; } = string.Empty;
    public string OwnerId { get; set; } = string.Empty;
}