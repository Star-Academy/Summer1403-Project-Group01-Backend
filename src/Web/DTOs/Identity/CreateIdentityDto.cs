using System.ComponentModel.DataAnnotations;

namespace Web.DTOs.Identity;

public class CreateIdentityDto
{
    [Required]
    public string Username { get; set; } = String.Empty;
    [Required]
    public string Password { get; set; } = String.Empty;
    [Required]
    public string Role { get; set; } = String.Empty;
}