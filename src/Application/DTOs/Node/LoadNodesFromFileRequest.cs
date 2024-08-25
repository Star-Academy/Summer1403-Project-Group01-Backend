using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Node;

public class LoadNodesFromFileRequest
{
    public string NodeType { get; set; } = null!;
    public string FilePath { get; set; } = null!;
    public Dictionary<string, string> Aliases { get; set; } = null!;
}