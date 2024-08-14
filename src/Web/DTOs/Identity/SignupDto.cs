using System.ComponentModel.DataAnnotations;

namespace Web.DTOs.Identity;

public class SignupDto
{
    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; } = String.Empty;
    [Required]
    [MaxLength(50)]
    public string LastName { get; set; } = String.Empty;
    [Required]
    [MaxLength(50)]
    [EmailAddress]
    public string Email { get; set; } = String.Empty;
    [Required]
    [MaxLength(50)]
    public string UserName { get; set; } = String.Empty;
    [Required]
    [MaxLength(50)]
    [MinLength(8)]
    public string Password { get; set; } = String.Empty;
    [Required]
    [MaxLength(50)]
    public string Role { get; set; } = String.Empty;
}