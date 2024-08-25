namespace Web.DTOs.Node;

public class ImportNodesDto
{
    public string NodeType { get; set; } = null!; 
    public Dictionary<string, string> Aliases { get; set; } = null!;
}