namespace Domain.Entities;

public class EdgeAttributeValue
{
    public long EdgeId { get; set; }
    
    public Edge? Edge { get; set; }
    
    public long EdgeAttributeId { get; set; }
    
    public EdgeAttribute? EdgeAttribute { get; set; }

    public string Value { get; set; } = string.Empty;
}