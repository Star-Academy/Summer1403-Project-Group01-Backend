using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Identity;

public class LoginUserRequest
{
    public string? UserName { get; set; }
    public string? Email { get; set; } 
    public string Password { get; set; } = String.Empty;
}