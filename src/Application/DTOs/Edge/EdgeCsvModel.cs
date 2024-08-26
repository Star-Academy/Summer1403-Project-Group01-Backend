namespace Application.DTOs.Edge;

public class EdgeCsvModel
{
    public long Id { get; set; }
    public long SourceValue { get; set; }
    public long DestinationValue { get; set; }
    public long TypeId { get; set; }
    public string AttributesJson { get; set; } = "{}";
}