using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Edge
{
    [Key]
    public long Id { get; set; }

    public long SourceValue { get; set; }

    public long DestinationValue { get; set; }
    
    public Node? SourceNode { get; set; }
    
    public Node? DestinationNode { get; set; }
    
    public long TypeId { get; set; }
    
    public EdgeType? Type { get; set; }
    
    public List<EdgeAttributeValue>? AttributeValues { get; set; }
}