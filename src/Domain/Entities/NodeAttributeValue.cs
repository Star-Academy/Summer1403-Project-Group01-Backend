using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class NodeAttributeValue
{
    [Key]
    public long Id { get; set; }
    public long NodeId { get; set; }
    public Node? Node { get; set; }
    public long NodeAttributeId { get; set; }
    public NodeAttribute? NodeAttribute { get; set; }
    [MaxLength(255)]
    public string Value { get; set; } = null!;
}