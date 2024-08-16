using Application.DTOs;
using Application.DTOs.Identity;
using Application.DTOs.Identity.ChangeRole;
using Application.ExtensionMethods;
using Application.Interfaces;
using Application.Interfaces.Services;
using Application.Mappers;
using Domain.Entities;
using Web.Interfaces;

namespace Application.Services;

public class IdentityService : IIdentityService
{
    private readonly IUserManager _userManager;
    private readonly IRoleManager _roleManager;
    private readonly IJwtGenerator _jwtGenerator;
    public IdentityService(IUserManager userManager,
        IRoleManager roleManager,
        IJwtGenerator jwtGenerator)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtGenerator = jwtGenerator;
    }

    public async Task<Result<CreateUserResponse>> SignUpUser(CreateUserRequest createUserRequest)
    {
        if (!await _roleManager.RoleExistsAsync(createUserRequest.Role))
        {
            return Result<CreateUserResponse>.Fail("role does not exist");
        }

        var appUser = createUserRequest.ToAppUser();
        
        var appUserResult = await _userManager.CreateAsync(appUser, createUserRequest.Password);
        if (!appUserResult.Succeeded)
        {
            return Result<CreateUserResponse>.Fail(appUserResult.Errors.FirstMessage());
        }
        
        var roleResult = await _userManager.SetRoleAsync(appUser, createUserRequest.Role);
        if (!roleResult.Succeeded)
        {
            return Result<CreateUserResponse>.Fail(roleResult.Errors.FirstMessage());
        }

        return Result<CreateUserResponse>.Ok(appUser.ToCreateUserResponse(createUserRequest.Role));
    }

    public async Task<Result<LoginUserResponse>> Login(LoginUserRequest loginUserRequest)
    {
        AppUser? appUser;

        if (!string.IsNullOrEmpty(loginUserRequest.UserName))
        {
            appUser = await _userManager.FindByNameAsync(loginUserRequest.UserName);
        }
        else if (!string.IsNullOrEmpty(loginUserRequest.Email))
        {
            appUser = await _userManager.FindByEmailAsync(loginUserRequest.Email);
        }
        else
        {
            return Result<LoginUserResponse>.Fail("You should enter email or username!");
        }

        if (appUser is null) return Result<LoginUserResponse>.Fail("Invalid username/email!");

        var succeed = await _userManager.CheckPasswordAsync(appUser, loginUserRequest.Password);

        if (!succeed) return Result<LoginUserResponse>.Fail("Username/Email not found and/or password incorrect");
        
        var role = await _userManager.GetRoleAsync(appUser);
        var token = _jwtGenerator.GenerateToken(appUser, role);

        return Result<LoginUserResponse>.Ok(appUser.ToLoginUserResponse(role, token));
    }

    public async Task<Result> ChangeRole(ChangeRoleRequest request)
    {
        AppUser? appUser = await _userManager.FindByNameAsync(request.UserName);

        if (appUser is null) return Result<LoginUserResponse>.Fail("Invalid username");

        var result = await _userManager.ChangeRoleAsync(appUser, request.Role);
        
        return result.Succeeded ? Result.Ok() : Result.Fail(result.Errors.FirstMessage());
    }
}