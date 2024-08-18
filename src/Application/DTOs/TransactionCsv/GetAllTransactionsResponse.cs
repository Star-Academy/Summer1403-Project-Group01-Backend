namespace Web.DTOs.Transaction;

public class GetAllTransactionsResponse
{
    public List<TransactionDto> Transactions { get; set; } = new();

    public class TransactionDto
    {
        public long TransactionId { get; set; }
        public long SourceAccountId { get; set; }
        public long DestinationAccountId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; } = string.Empty;
    }

}