using Application.DTOs;
using Application.DTOs.Identity;
using Application.Interfaces;
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
        
        var appUser = new AppUser
        {
            FirstName = createUserRequest.FirstName,
            LastName = createUserRequest.LastName,
            Email = createUserRequest.Email,
            UserName = createUserRequest.UserName
        };
        
        var appUserResult = await _userManager.CreateAsync(appUser, createUserRequest.Password);
        if (!appUserResult.Succeeded)
        {
            return Result<CreateUserResponse>.Fail(appUserResult.Errors.ToString());
        }
        
        var roleResult = await _userManager.AddToRoleAsync(appUser, createUserRequest.Role);
        if (!roleResult.Succeeded)
        {
            return Result<CreateUserResponse>.Fail(roleResult.Errors.ToString());
        }

        return Result<CreateUserResponse>.Ok(new CreateUserResponse
        {
            FirstName = appUser.FirstName,
            LastName = appUser.LastName,
            Email = appUser.Email,
            UserName = appUser.UserName,
            Role = createUserRequest.Role
        });
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

        return Result<LoginUserResponse>.Ok(new LoginUserResponse
        {
            UserName = appUser.UserName,
            Token = _jwtGenerator.GenerateToken(appUser, role)
        });
    }

}