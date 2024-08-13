using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.JavaScript;

namespace Web.DTOs.Identity;

public class LoginDto
{
    [Required]
    public string UserName { get; set; } = String.Empty;
    [Required]
    public string Password { get; set; } = String.Empty;
}