using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class EdgeType
{
    [Key]
    public long Id { get; set; }

    public string Label { get; set; } = string.Empty;
    
    public long SourceNodeAttributeId { get; set; }
    
    public long DestinationNodeAttributeId { get; set; }
    
    public NodeAttribute? SourceNodeAttribute { get; set; }
    
    public NodeAttribute? DestinationNodeAttribute { get; set; }

}