using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Edge
{
    [Key]
    public long Id { get; set; }

    public string SourceValue { get; set; } = string.Empty;

    public string DestinationValue { get; set; } = string.Empty;
    
    public Node? SourceNode { get; set; }
    
    public Node? DestinationNode { get; set; }
    
    public long TypeId { get; set; }
    
    public EdgeType? Type { get; set; }
    
    public List<EdgeAttributeValue>? AttributeValues { get; set; }
}