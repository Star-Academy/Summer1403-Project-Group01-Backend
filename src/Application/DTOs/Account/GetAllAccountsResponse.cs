namespace Application.DTOs.Account;

public class GetAllAccountsResponse
{
    public List<AccountCsvModel> Accounts { get; set; } = new();
}