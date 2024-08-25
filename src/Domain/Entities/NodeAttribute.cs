using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class NodeAttribute
{
    [Key] 
    public long Id { get; set; }
    [MaxLength(255)]
    public string Label { get; set; } = null!;
    public long NodeTypeId { get; set; }
    public NodeType? NodeType { get; set; }
}