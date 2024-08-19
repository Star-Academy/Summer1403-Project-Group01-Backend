using Application.DTOs;
using Application.DTOs.Identity;
using Application.DTOs.Identity.ChangeRole;
using Application.ExtensionMethods;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Mappers;
using Domain.Entities;

namespace Application.Services;

public class IdentityService : IIdentityService
{
    private readonly IUserManagerRepository _userManagerRepository;
    private readonly IRoleManagerRepository _roleManagerRepository;
    private readonly IJwtGenerator _jwtGenerator;
    public IdentityService(IUserManagerRepository userManagerRepository,
        IRoleManagerRepository roleManagerRepository,
        IJwtGenerator jwtGenerator)
    {
        _userManagerRepository = userManagerRepository;
        _roleManagerRepository = roleManagerRepository;
        _jwtGenerator = jwtGenerator;
    }

    public async Task<Result<CreateUserResponse>> SignUpUser(CreateUserRequest createUserRequest)
    {
        if (!await _roleManagerRepository.RoleExistsAsync(createUserRequest.Role))
        {
            return Result<CreateUserResponse>.Fail("role does not exist");
        }

        var appUser = createUserRequest.ToAppUser();
        
        var appUserResult = await _userManagerRepository.CreateAsync(appUser, createUserRequest.Password);
        if (!appUserResult.Succeeded)
        {
            return Result<CreateUserResponse>.Fail(appUserResult.Errors.FirstMessage());
        }
        
        var roleResult = await _userManagerRepository.SetRoleAsync(appUser, createUserRequest.Role);
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
            appUser = await _userManagerRepository.FindByNameAsync(loginUserRequest.UserName);
        }
        else if (!string.IsNullOrEmpty(loginUserRequest.Email))
        {
            appUser = await _userManagerRepository.FindByEmailAsync(loginUserRequest.Email);
        }
        else
        {
            return Result<LoginUserResponse>.Fail("You should enter email or username!");
        }

        if (appUser is null) return Result<LoginUserResponse>.Fail("Invalid username/email!");

        var succeed = await _userManagerRepository.CheckPasswordAsync(appUser, loginUserRequest.Password);

        if (!succeed) return Result<LoginUserResponse>.Fail("Username/Email not found and/or password incorrect");
        
        var role = await _userManagerRepository.GetRoleAsync(appUser);
        var token = _jwtGenerator.GenerateToken(appUser, role);

        return Result<LoginUserResponse>.Ok(appUser.ToLoginUserResponse(role, token));
    }

    public async Task<Result> ChangeRole(ChangeRoleRequest request)
    {
        if (!await _roleManagerRepository.RoleExistsAsync(request.Role))
        {
            return Result.Fail("role does not exist");
        }
        AppUser? appUser = await _userManagerRepository.FindByNameAsync(request.UserName);

        if (appUser is null) return Result<LoginUserResponse>.Fail("Invalid username");

        var result = await _userManagerRepository.ChangeRoleAsync(appUser, request.Role);
        
        return result.Succeeded ? Result.Ok() : Result.Fail(result.Errors.FirstMessage());
    }
}