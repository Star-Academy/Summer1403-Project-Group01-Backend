namespace Application.DTOs.TransactionCsv;

public class GetAllTransactionsResponse
{
    public List<TransactionCsvModel> Transactions { get; set; } = new();
}