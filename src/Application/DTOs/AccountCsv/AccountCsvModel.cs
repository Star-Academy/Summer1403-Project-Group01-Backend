namespace Application.DTOs.AccountCsv;

public class AccountCsvModel
{
    public long AccountID { get; set; }
    public long CardID { get; set; }
    public string IBAN { get; set; } = String.Empty;
    public string AccountType { get; set; } = String.Empty;
    public string BranchTelephone { get; set; } = String.Empty;
    public string BranchAdress { get; set; } = String.Empty;
    public string BranchName { get; set; } = String.Empty;
    public string OwnerName { get; set; } = String.Empty;
    public string OwnerLastName { get; set; } = String.Empty;
    public string OwnerID { get; set; } = String.Empty;
}