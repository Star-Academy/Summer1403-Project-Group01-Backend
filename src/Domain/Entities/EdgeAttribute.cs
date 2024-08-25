using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class EdgeAttribute
{
    [Key] 
    public long Id { get; set; }

    public string Label { get; set; } = string.Empty;
    
    public long EdgeTypeId { get; set; }
    
    public EdgeType? EdgeTypee { get; set; }
}