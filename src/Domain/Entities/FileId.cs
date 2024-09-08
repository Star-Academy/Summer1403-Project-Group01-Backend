using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("FileIds")]
public class FileId
{
    [Key]
    public long Id { get; set; }
}