using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services;

public class UserManagerService : IUserManager
{
    private readonly UserManager<AppUser> _userManager;

    public UserManagerService(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IdentityResult> CreateAsync(AppUser user, string password)
    {
        return await _userManager.CreateAsync(user, password);
    }

    public async Task<IdentityResult> AddToRoleAsync(AppUser user, string role)
    {
        return await _userManager.AddToRoleAsync(user, role);
    }

    public async Task<IdentityResult> SetRoleAsync(AppUser user, string role)
    {
        // TODO check if adding multiple roles
        return await _userManager.AddToRoleAsync(user, role);
    }

    public async Task<AppUser?> FindByNameAsync(string userName)
    {
        return await _userManager.FindByNameAsync(userName);
    }

    public async Task<AppUser?> FindByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }

    public async Task<AppUser?> FindByIdAsync(string userId)
    {
        return await _userManager.FindByIdAsync(userId);
    }

    public async Task<IdentityResult> UpdateAsync(AppUser user)
    {
        return await _userManager.UpdateAsync(user);
    }

    public async Task<IdentityResult> ChangePasswordAsync(AppUser user, string currentPassword, string newPassword)
    {
        return await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
    }

    public async Task<string> GetRoleAsync(AppUser user)
    {
        // TODO can return null here.
        return (await _userManager.GetRolesAsync(user)).Single();
    }

    public async Task<bool> CheckPasswordAsync(AppUser user, string password)
    {
        // TODO there should be a sign in manager service
        return await _userManager.CheckPasswordAsync(user, password);
    }
}