using Application.Interfaces;
using Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Repositories;

public class RoleManagerRepositoryRepository : IRoleManagerRepository
{
    private readonly RoleManager<IdentityRole> _roleManager;
    public RoleManagerRepositoryRepository(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<bool> RoleExistsAsync(string roleName)
    {
        return await _roleManager.RoleExistsAsync(roleName);
    }
}