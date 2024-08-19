namespace Application.DTOs.AccountCsv;

public class GetAllAccountsResponse
{
    public List<AccountCsvModel> Accounts { get; set; } = new();
}