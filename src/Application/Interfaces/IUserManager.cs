using Application.DTOs;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Interfaces;

public interface IUserManager
{ 
    Task<IdentityResult> CreateAsync(AppUser user, string password);
    Task<IdentityResult> SetRoleAsync(AppUser user, string role);
    Task<AppUser?> FindByNameAsync(string userName);
    Task<AppUser?> FindByEmailAsync(string email);
    Task<AppUser?> FindByIdAsync(string userId);
    Task<IdentityResult> UpdateAsync(AppUser user);
    Task<IdentityResult> ChangePasswordAsync(AppUser user, string currentPassword, string newPassword);
    Task<string> GetRoleAsync(AppUser user);
    Task<Result> CheckPasswordAsync(AppUser user, string password, bool lockoutOnFailure);
}