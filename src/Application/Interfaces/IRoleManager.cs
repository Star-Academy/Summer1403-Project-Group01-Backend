namespace Application.Interfaces;

public interface IRoleManager
{
    Task<bool> RoleExistsAsync(string roleName);
}