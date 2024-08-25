using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class NodeType
{
    [Key] 
    public long Id { get; set; }
    
    public string Label { get; set; } = string.Empty;
}