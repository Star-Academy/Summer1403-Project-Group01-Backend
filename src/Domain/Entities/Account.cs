using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;
[Table("Accounts")]
public class Account
{
    [Key]
    public long AccountId { get; set; }
    public long CardId { get; set; }
    
    [MaxLength(50)]
    public string Iban { get; set; } = string.Empty;

    [MaxLength(50)]
    public string AccountType { get; set; } = string.Empty;

    [MaxLength(20)]
    public string BranchTelephone { get; set; } = string.Empty;

    [MaxLength(150)]
    public string BranchAddress { get; set; } = string.Empty;

    [MaxLength(50)]
    public string BranchName { get; set; } = string.Empty;

    [MaxLength(50)]
    public string OwnerName { get; set; } = string.Empty;

    [MaxLength(50)]
    public string OwnerLastName { get; set; } = string.Empty;

    [MaxLength(50)]
    public string OwnerId { get; set; } = string.Empty;

    public List<Transaction> SourceTransactions { get; set; } = new();
    public List<Transaction> DestinationTransactions { get; set; } = new();
}