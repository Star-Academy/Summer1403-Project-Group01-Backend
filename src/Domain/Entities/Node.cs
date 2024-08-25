using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Node
{
    [Key] 
    public long Id { get; set; }
    public long TypeId { get; set; }
    public NodeType? Type { get; set; }
    public List<NodeAttributeValue> AttributeValues { get; set; } = null!;
}