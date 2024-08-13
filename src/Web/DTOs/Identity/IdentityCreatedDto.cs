namespace Web.DTOs.Identity;

public class IdentityCreatedDto
{
    public string FirstName { get; set; } = String.Empty;
    public string LastName { get; set; } = String.Empty;
    public string Email { get; set; } = String.Empty;
    public string Username { get; set; } = String.Empty;
    public string Role { get; set; } = String.Empty;
}