using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.JavaScript;

namespace Web.DTOs.Identity;

public class LoginDto
{
    public string? UserName { get; set; }
    public string? Email { get; set; } 
    [Required]
    public string? Password { get; set; }
}