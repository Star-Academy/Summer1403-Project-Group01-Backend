namespace Application.DTOs.Identity.ChangeRole;

public class ChangeRoleRequest
{
    public string UserName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}