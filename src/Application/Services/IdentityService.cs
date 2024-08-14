using Application.DTOs;
using Application.DTOs.Identity;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Services;

public class IdentityService
{
    private readonly IUserManager _userManager;
    private readonly IRoleManager _roleManager;
    private readonly IJwtGenerator _jwtGenerator;
    public IdentityService(IUserManager userManager,
        IRoleManager roleManager,
        IJwtGenerator jwtGenerator
        )
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtGenerator = jwtGenerator;
    }

    public async Task<Result<AppUser>> SignUp(CreateIdentityDto createIdentityDto)
    {
        if (!await _roleManager.RoleExistsAsync(createIdentityDto.Role))
        {
            return Result<AppUser>.Fail("role does not exist");
        }
        
        var appUser = new AppUser
        {
            FirstName = createIdentityDto.FirstName,
            LastName = createIdentityDto.LastName,
            Email = createIdentityDto.Email,
            UserName = createIdentityDto.Username
        };
        
        var appUserResult = await _userManager.CreateAsync(appUser, createIdentityDto.Password);
        if (!appUserResult.Succeeded)
        {
            return Result<AppUser>.Fail("Failed to create user");
        }
        
        var roleResult = await _userManager.AddToRoleAsync(appUser, createIdentityDto.Role);
        if (!roleResult.Succeeded)
        {
            return Result<AppUser>.Fail("role does not exist");
        }

        return Result<AppUser>.Ok(appUser);
    }

    public async Task<Result<AppUser>> Login(LoginDto loginDto)
    {
        AppUser? user;

        if (!string.IsNullOrEmpty(loginDto.UserName))
        {
            user = await _userManager.FindByNameAsync(loginDto.UserName);
        }
        else if (!string.IsNullOrEmpty(loginDto.Email))
        {
            user = await _userManager.FindByEmailAsync(loginDto.Email);
        }
        else
        {
            return Unauthorized("You should enter email or username!");
        }

        if (user is null) return Unauthorized("Invalid username/email!");

        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

        if (!result.Succeeded) return Unauthorized("Username/Email not found and/or password incorrect");
        
        var roles = await _userManager.GetRolesAsync(user);

    }

}