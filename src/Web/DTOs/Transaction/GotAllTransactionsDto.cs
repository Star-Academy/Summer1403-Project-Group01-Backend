using System.ComponentModel.DataAnnotations;

namespace Web.DTOs.Transaction;

public class GotAllTransactionsDto
{
    public long TransactionId { get; set; }
    public long SourceAccountId { get; set; }
    public long DestinationAccountId { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string Type { get; set; } = string.Empty;
}