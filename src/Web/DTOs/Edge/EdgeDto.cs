namespace Web.DTOs.Edge;

public class EdgeDto
{
    public long Id { get; set; }
    public long SourceValue { get; set; }
    public long DestinationValue { get; set; }
    public long TypeId { get; set; }
    public string TypeLabel { get; set; } = String.Empty;
    public Dictionary<string, string> Attributes { get; set; } = new();
}