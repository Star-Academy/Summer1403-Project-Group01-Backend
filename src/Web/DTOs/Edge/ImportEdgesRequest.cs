using System.ComponentModel.DataAnnotations;

namespace Web.DTOs.Edge;

public class ImportEdgesRequest
{
    [Required]
    public IFormFile File { get; set; }

    [Required] public string SourceColumn { get; set; } = String.Empty;

    [Required]
    public string DestinationColumn { get; set; } = String.Empty;

    [Required]
    public string TypeLabelColumn { get; set; } = String.Empty;

    [Required]
    public string IdColumn { get; set; } = String.Empty;
}