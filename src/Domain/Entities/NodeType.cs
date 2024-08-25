using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class NodeType
{
    [Key] 
    public long Id { get; set; }
    [MaxLength(255)]
    public string Label { get; set; } = null!;
    public List<Node> Nodes { get; set; } = null!;
    public List<NodeAttribute> Attributes { get; set; } = null!;
}