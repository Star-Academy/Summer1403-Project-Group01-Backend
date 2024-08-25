namespace Application.DTOs.NodeAttribute;

public class CreateNodeAttributeRequest
{
    public string NodeType { get; set; } = null!;
    public string Label { get; set; } = null!;
}