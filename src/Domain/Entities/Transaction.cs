using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;
[Table("Transactions")]
public class Transaction
{
    public long TransactionId { get; set; }
    public long SourceAccountId { get; set; }
    public Account SourceAccount { get; set; }
    public long DestinationAccountId { get; set; }
    public Account DestinationAccount { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string Type { get; set; } = String.Empty;
    
    
}