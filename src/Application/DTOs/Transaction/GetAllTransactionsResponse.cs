namespace Application.DTOs.Transaction;

public class GetAllTransactionsResponse
{
    public List<TransactionCsvModel> Transactions { get; set; } = new();
}