using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class EdgeAttribute
{
    [Key] 
    public long Id { get; set; }

    public string Label { get; set; } = string.Empty;
    
    public long EdgeTypeId { get; set; }
    
    public EdgeType? EdgeType { get; set; }
    public List<EdgeAttributeValue>? EdgeAttributeValues { get; set; }
}