using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;
[Table("Accounts")]
public class Transaction
{
    public int TransactionId { get; set; }
    public int SourceAccountId { get; set; }
    public int DestinationAccountId { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string Type { get; set; } = String.Empty;
    public Account SourceAccount { get; set; }
    public Account DestinationAccount { get; set; }
}