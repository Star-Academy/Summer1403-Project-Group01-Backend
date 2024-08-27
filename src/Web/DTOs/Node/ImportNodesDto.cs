using System.ComponentModel.DataAnnotations;

namespace Web.DTOs.Node;

public class ImportNodesDto
{
    [Required]
    public string NodeType { get; set; } = null!; 
    [Required]
    public Dictionary<string, string> Aliases { get; set; } = [];
    [Required] 
    public IFormFile File { get; set; } = null!;
}