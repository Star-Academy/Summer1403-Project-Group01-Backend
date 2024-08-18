using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;
[Table("Accounts")]
public class Account
{
    public long AccountId { get; set; }
    public long CardId { get; set; }
    public string Iban { get; set; } = String.Empty;
    public string AccountType { get; set; } = String.Empty;
    public string BranchTelephone { get; set; } = String.Empty;
    public string BranchAddress { get; set; } = String.Empty;
    public string BranchName { get; set; } = String.Empty;
    public string OwnerName { get; set; } = String.Empty;
    public string OwnerLastName { get; set; } = String.Empty;
    public string OwnerId { get; set; } = String.Empty;

    public List<Transaction> SourceTransactions { get; set; } = new();
    public List<Transaction> DestinationTransactions { get; set; } = new();
}