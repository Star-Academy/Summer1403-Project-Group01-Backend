namespace Application.Interfaces.Services;

public interface IAccountService
{
    Task AddAccountsFromCsvAsync(string filePath);
}